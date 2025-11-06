using Microsoft.AspNetCore.Mvc;
using MinijuegosAPI.Data;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using MinijuegosAPI.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using MinijuegosAPI.DTOs;

namespace MinijuegosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinijuegosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MinijuegosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // mapa mental: muerte cerebral hoy 05/11
        // Factory implementado, ahora necesitaria implementar strategy para saber que tipo de pregunta devuelvo, ya que tienen distintos "cuerpos"

        /*[HttpGet("pregunta")]
        public ActionResult<Pregunta> pregunta(string tipo) 
        {
            if (tipo == null) 
            {
                return BadRequest();
            }
            
            MiniJuegoFactory factory = new MiniJuegoFactory(_context); // aca ocurre la magia, el factory esta acaaaaaa
            IMiniJuego juego = factory.GenerarMiniJuego(tipo);
            Pregunta pregunta = juego.GenerarPregunta();

            return Ok(pregunta);
        }*/

        [HttpGet("pregunta")]
        public ActionResult<Object> pregunta(string tipo)
        {
            if (tipo == null)
            {
                return BadRequest();
            }

            MiniJuegoFactory factory = new MiniJuegoFactory(_context); // aca ocurre la magia, el factory esta acaaaaaa
            IMiniJuego juego = factory.GenerarMiniJuego(tipo);
            Pregunta pregunta = juego.GenerarPregunta();

            // aca no me dio la cabeza para hacer el strategy y para poder devolver un solo tipo de dto acorde al tipo de pregunta arme el metodo mapear, que arma el dto
            // con los datos que precisa la vista y lo devolvemos como objeto nomas.
            // Despues habria que hacerlo bien pero por ahora es un strategy rustico como la tota Lugano
            object dto = mapearDTOcorrecto(pregunta);

            return Ok(dto);
        }

        public static object mapearDTOcorrecto(Pregunta p)
        {
            CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

            if(p.Tipo == "Logica")
            {
                JuegoLogicaResponseDTO juegoLogicaRes = new JuegoLogicaResponseDTO
                {
                    Id = p.Id,
                    TipoPregunta = p.Tipo,
                    Secuencia = cuerpoPreg.SecuenciaNumeros,
                    Pregunta = cuerpoPreg.Pregunta,
                    CodigoPregunta = p.Codigo,
                    FechaCreacion = p.FechaCreacion,
                    
                };

                return juegoLogicaRes;
            }
            else
            {
                return new { };
            }
        }
       
        // clase anidada aca para poder deserealizar el cuerpo de la preg y armar el dto que corresponde para mandar a la vista
        public class CuerpoLogica
        {
            [JsonPropertyName("secuenciaNumeros")]
            public int[] SecuenciaNumeros { get; set; } = Array.Empty<int>();

            [JsonPropertyName("pregunta")]
            public string Pregunta { get; set; } = "";
        }

        /*[HttpPost]
        public async Task<> validar()
        {

        }*/
    }
}

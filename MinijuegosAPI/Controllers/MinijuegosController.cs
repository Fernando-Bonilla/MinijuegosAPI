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

        [HttpGet("pregunta")]
        public ActionResult<Object> pregunta(string? tipo)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                return BadRequest(new { Mensaje = "El Parametro tipo es requerido" });
            }

            MiniJuegoFactory factory = new MiniJuegoFactory(_context); // aca ocurre la magia, el factory esta acaaaaaa
            IMiniJuego juego = factory.GenerarMiniJuego(tipo);

            if (juego == null)
            {
                return BadRequest(new { Mensaje = $"El tipo {tipo} no es valido" });
            }

            Pregunta pregunta = juego.GenerarPregunta();

            if (pregunta == null || string.IsNullOrEmpty(pregunta.CuerpoPregunta))
            {                
                return StatusCode(500, new {Mensje = "No se pudo generar la pregunta"});
            }
            
            // aca no me dio la cabeza para hacer el strategy y para poder devolver un solo tipo de dto acorde al tipo de pregunta arme el metodo mapear, que arma el dto
            // con los datos que precisa la vista y lo devolvemos como objeto nomas.
            // Despues habria que hacerlo bien pero por ahora es un strategy rustico como la tota Lugano
            object dto = mapearDTOcorrecto(pregunta);

            return Ok(dto);
        }

        public static object mapearDTOcorrecto(Pregunta p)
        {
            //CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

            if(p.Tipo == "Logica")
            {
                CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

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
            else if (p.Tipo == "Memoria")
            {
                CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

                JuegoMemoriaResponseDTO juegoMemoriaRes = new JuegoMemoriaResponseDTO
                {
                    Id = p.Id,
                    TipoPregunta = p.Tipo,
                    Secuencia = cuerpoPreg.SecuenciaNumeros,
                    Pregunta = cuerpoPreg.Pregunta,
                    CodigoPregunta = p.Codigo,
                    FechaCreacion = p.FechaCreacion,
                };

                return juegoMemoriaRes;
            }
            else if(p.Tipo == "Matematica")
            {
                CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

                JuegoMatematicaResponseDTO juegoMatematicaRes = new JuegoMatematicaResponseDTO
                {
                    Id = p.Id,
                    TipoPregunta = p.Tipo,
                    Secuencia = cuerpoPreg.SecuenciaNumeros,
                    Pregunta = cuerpoPreg.Pregunta,                    
                    FechaCreacion = p.FechaCreacion,
                };

                return juegoMatematicaRes;

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

        [HttpPost("Validar")]
        public ActionResult<ValidacionResponseDTO> Validar([FromBody] ValidacionRequestDTO dto)
        {
            if (dto.Id == 0)
            {
                return BadRequest(new {Mensaje= $"Valor Id: {dto.Id} no valido, por favor ingrese un valor mayor a 0"});
            }

            Pregunta? pregunta = _context.Preguntas
                .FirstOrDefault(p => p.Id == dto.Id);

            if (pregunta == null)
            {
                return StatusCode(404, "Pregunta no encontrada");
            }

            MiniJuegoFactory factory = new MiniJuegoFactory(_context);
            IMiniJuego? juego = factory.GenerarMiniJuego(pregunta.Tipo);

            if (juego == null)
            {
                return BadRequest(new { Mensaje = $"El tipo {pregunta.Tipo} no es valido" });
            }            

            ResultadoValidacion resultValid = juego.ValidarRespuesta(pregunta, dto.Respuesta);

            //ValidacionResponseDTO validacionResponseDTO = new ValidacionResponseDTO(); // esto por ahora es para poder poner algo que devuelve
            ValidacionResponseDTO validacionResponseDTO = new ValidacionResponseDTO 
            {
                EsCorrecta = resultValid.EsCorrecta,
                Mensaje = resultValid.Mensaje,
                RespuestaCorrecta = resultValid.RespuestaCorrecta,
                TipoMiniJuego = pregunta.Tipo,

            };

            return validacionResponseDTO;
        }
    }
}

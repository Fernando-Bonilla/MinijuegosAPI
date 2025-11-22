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
        private readonly IMiniJuegoFactory _factory;
        public MinijuegosController(ApplicationDbContext context, IMiniJuegoFactory factory)
        {
            _context = context;
            _factory = factory;
        }        

        [HttpGet("pregunta")]
        public ActionResult<Object> pregunta(string? tipo)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                return BadRequest(new { Mensaje = "El Parametro tipo es requerido" });
            }

            //MiniJuegoFactory factory = new MiniJuegoFactory(_context); 
            //IMiniJuego juego = factory.GenerarMiniJuego(tipo);
            IMiniJuego juego = _factory.GenerarMiniJuego(tipo);


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
            object dto = Utils.mapearDTOcorrecto(pregunta);

            return Ok(dto);
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

            //MiniJuegoFactory factory = new MiniJuegoFactory(_context);
            IMiniJuego? juego = _factory.GenerarMiniJuego(pregunta.Tipo);

            if (juego == null)
            {
                return BadRequest(new { Mensaje = $"El tipo {pregunta.Tipo} no es valido" });
            }            

            if(dto.Respuesta == null)
            {
                return BadRequest(new { Mensaje = $"La respuesta no puede ser nula" });
            }

            if(!Utils.ValidarEntradaRespuesta(pregunta, dto.Respuesta))
            {
                return BadRequest(new { Mensaje = "Respuesta invalida" });
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

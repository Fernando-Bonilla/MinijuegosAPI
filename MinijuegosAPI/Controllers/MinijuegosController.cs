using Microsoft.AspNetCore.Mvc;
using MinijuegosAPI.Data;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using MinijuegosAPI.Services;

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
        /*public IActionResult Index()
        {
            return View();
        }*/

        // mapa mental:
        // La Controller tiene los endopoints
        // El get recibe el tipo ej: "Logica"
        // El get con ese parametro tipo, llama al factory el factory segun lo que recibe elije que minijuego (pregunta) crear
        // el Service le pide al Factory que Strategy necesita para ese tipo, es decir, siempre vamos a generar un objeto Tipo Pregunta pero la strategy nos va a decir como armar esa pregunta

        [HttpGet("pregunta")]
        public ActionResult<Pregunta> pregunta(string tipo) 
        {
            tipo = "logica";
            MiniJuegoFactory factory = new MiniJuegoFactory(_context); // aca ocurre la magia, el factory esta acaaaaaa
            IMiniJuego juego = factory.GenerarMiniJuego(tipo);
            Pregunta pregunta = juego.GenerarPregunta();

            return Ok(pregunta);

        }

        /*[HttpPost]
        public async Task<> validar()
        {

        }*/
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MinijuegosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinijuegosController : Controller
    {
        /*public IActionResult Index()
        {
            return View();
        }*/

        // mapa mental:
        // La Controller tiene los endopoints
        // El get recibe el tipo ej: "Logica"
        // El get con ese parametro tipo, llama al Servicio "GenerarPregunta(tipo)"
        // el Service le pide al Factory que Strategy necesita para ese tipo, es decir, siempre vamos a generar un objeto Tipo Pregunta pero la strategy nos va a decir como armar esa pregunta

        [HttpGet("pregunta")]
        public async Task<string> pregunta(string tipo) 
        {
            string resp = $"Hola: {tipo}";
            return resp;
        }

        /*[HttpPost]
        public async Task<> validar()
        {

        }*/
    }
}

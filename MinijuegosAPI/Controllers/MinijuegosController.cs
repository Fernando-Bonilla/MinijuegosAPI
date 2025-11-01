using Microsoft.AspNetCore.Mvc;

namespace MinijuegosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MinijuegosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

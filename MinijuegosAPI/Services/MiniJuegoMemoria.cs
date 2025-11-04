using MinijuegosAPI.Data;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;

namespace MinijuegosAPI.Services
{
    public class MiniJuegoMemoria : IMiniJuego
    {
        private readonly ApplicationDbContext _context;
        public MiniJuegoMemoria(ApplicationDbContext context)
        {
            _context = context;
        }
        public Pregunta GenerarPregunta() 
        {
            return new Pregunta();
        }
    }
}

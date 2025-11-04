using MinijuegosAPI.Data;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;

namespace MinijuegosAPI.Services
{
    public class MiniJuegoMatematica : IMiniJuego
    {
        private readonly ApplicationDbContext _context;
        public MiniJuegoMatematica(ApplicationDbContext context)
        {
            _context = context;
        }

        public Pregunta GenerarPregunta()
        {
            return new Pregunta();
        }

    }
}

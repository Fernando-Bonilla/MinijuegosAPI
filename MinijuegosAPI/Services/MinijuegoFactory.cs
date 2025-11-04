using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Data;
using MinijuegosAPI.Services;

namespace MinijuegosAPI.Services
{
    public class MiniJuegoFactory : IMiniJuegoFactory
    {
        private readonly ApplicationDbContext _context;

        public MiniJuegoFactory(ApplicationDbContext context)
        {
            _context = context;
        }


        public IMiniJuego GenerarMiniJuego(string tipo)
        {
            if (tipo == null)
            {
                throw new ArgumentNullException(nameof(tipo));
            }

            if(tipo == "matematica")
            {
                return new MiniJuegoMatematica(_context);
            }
            else if (tipo == "logica")
            {
                return new MiniJuegoLogica(_context);
            }
            else
            {
                return new MiniJuegoMemoria(_context);
            }
        }
    }
}

using MinijuegosAPI.Interfaces;

namespace MinijuegosAPI.Services
{
    public class MinijuegoFactory : IMinijuegoFactory
    {
        private readonly IMinijuegoStrategy _matematica;
        private readonly IMinijuegoStrategy _logica;
        private readonly IMinijuegoStrategy _memoria;
        public MinijuegoFactory(IMinijuegoStrategy matematica, IMinijuegoStrategy logica, IMinijuegoStrategy memoria) 
        { 
            _matematica = matematica;
            _logica = logica;
            _memoria = memoria;
        }

        public IMinijuegoStrategy Obtener( string tipo)
        {
            if (tipo == null) 
            {
                throw new ArgumentNullException("El tipo es requerido.", nameof(tipo));
            }

            if(tipo == "matematica")
            {
                return _matematica;
            }else if ( tipo == "logica")
            {
                return _logica;
            }else
            {
                return _memoria;
            }
        }
    }
}

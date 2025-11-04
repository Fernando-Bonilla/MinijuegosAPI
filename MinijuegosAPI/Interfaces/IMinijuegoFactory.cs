using MinijuegosAPI.Interfaces;

namespace MinijuegosAPI.Interfaces
{
    public interface IMiniJuegoFactory 
    {
        public IMiniJuego GenerarMiniJuego(string tipo);
        
    }
}

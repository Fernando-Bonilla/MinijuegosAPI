using MinijuegosAPI.Models;

namespace MinijuegosAPI.Interfaces
{
    public interface IMiniJuego
    {
        Pregunta GenerarPregunta(); // Mi Interfaz que obliga a implementar el metodo en cada Clase que implemente esta interfaz
    }
}

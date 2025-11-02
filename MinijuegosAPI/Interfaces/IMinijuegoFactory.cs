namespace MinijuegosAPI.Interfaces
{
    public interface IMinijuegoFactory
    {
        IMinijuegoStrategy Obtener(string tipo); // este factory va a decidir que tipo de strategy usamos segun el tipo de minijuego q recibe, las clases que implementa esta interfaz
    }
}

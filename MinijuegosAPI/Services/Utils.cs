using MinijuegosAPI.Models;

namespace MinijuegosAPI.Services
{
    public class Utils
    {
        public static bool ValidarEntradaRespuesta(Pregunta pregunta, string respuesta) 
        {
            if(respuesta == null)
            {
                return false;
            }

            respuesta = respuesta.Trim().ToLower();

            switch (pregunta.Tipo)
            {
                case "Matematica":
                    int parseado;

                    if (int.TryParse(respuesta, out parseado))
                    {
                        return true;
                    }
                    else
                        return false;

                case "Logica":
                    if (respuesta != "verdadero" && respuesta != "falso" && respuesta != "true" && respuesta != "false")
                    {
                        return false;
                    }
                    else
                        return true;

                case "Memoria":
                    if (respuesta != "si" && respuesta != "sí" && respuesta != "no" && respuesta != "true" && respuesta != "false")
                    {
                        return false;
                    }
                    else
                        return true;

                default: return false;
            }           
        }
    }
}

using MinijuegosAPI.DTOs;
using System.Text.Json;
using MinijuegosAPI.Models;
using System.Text.Json.Serialization;

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

        public static object mapearDTOcorrecto(Pregunta p)
        {
            if (p.Tipo == "Logica")
            {
                CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

                JuegoLogicaResponseDTO juegoLogicaRes = new JuegoLogicaResponseDTO
                {
                    Id = p.Id,
                    TipoPregunta = p.Tipo,
                    Secuencia = cuerpoPreg.SecuenciaNumeros,
                    Pregunta = cuerpoPreg.Pregunta,
                    CodigoPregunta = p.Codigo,
                    FechaCreacion = p.FechaCreacion,

                };

                return juegoLogicaRes;
            }
            else if (p.Tipo == "Memoria")
            {
                CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

                JuegoMemoriaResponseDTO juegoMemoriaRes = new JuegoMemoriaResponseDTO
                {
                    Id = p.Id,
                    TipoPregunta = p.Tipo,
                    Secuencia = cuerpoPreg.SecuenciaNumeros,
                    Pregunta = cuerpoPreg.Pregunta,
                    CodigoPregunta = p.Codigo,
                    FechaCreacion = p.FechaCreacion,
                };

                return juegoMemoriaRes;
            }
            else if (p.Tipo == "Matematica")
            {
                CuerpoLogica cuerpoPreg = JsonSerializer.Deserialize<CuerpoLogica>(p.CuerpoPregunta);

                JuegoMatematicaResponseDTO juegoMatematicaRes = new JuegoMatematicaResponseDTO
                {
                    Id = p.Id,
                    TipoPregunta = p.Tipo,
                    Secuencia = cuerpoPreg.SecuenciaNumeros,
                    Pregunta = cuerpoPreg.Pregunta,
                    FechaCreacion = p.FechaCreacion,
                };

                return juegoMatematicaRes;

            }

            else
            {
                return new { };
            }
        }

        // clase anidada aca para poder deserealizar el cuerpo de la preg y armar el dto que corresponde para mandar a la vista
        public class CuerpoLogica
        {
            [JsonPropertyName("secuenciaNumeros")]
            public int[] SecuenciaNumeros { get; set; } = Array.Empty<int>();

            [JsonPropertyName("pregunta")]
            public string Pregunta { get; set; } = "";
        }
    }
}

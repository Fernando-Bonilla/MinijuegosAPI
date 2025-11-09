using MinijuegosAPI.Data;
using MinijuegosAPI.DTOs;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using System.Text.Json;

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
            //return new Pregunta();
            JuegoMemoriaDTO juego = CrearJuegoDeMemoria();

            CuerpoMemoria cuerpoPregunta = new CuerpoMemoria
            {
                SecuenciaNumeros = new int[] { juego.Num1, juego.Num2, juego.Num3, juego.Num4, juego.Num5},
                Pregunta = juego.Pregunta
            };

            //string json = JsonSerializer.Serialize( cuerpoPregunta, new JsonSerializerOptions());
            string json = JsonSerializer.Serialize(cuerpoPregunta, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Pregunta pregunta = new Pregunta
            {
                Tipo = "Memoria",
                Codigo = juego.CodigoPregunta,
                CuerpoPregunta = json,
            };

            _context.Preguntas.Add( pregunta );
            _context.SaveChanges();

            return pregunta;
            
        }

        public static JuegoMemoriaDTO CrearJuegoDeMemoria()
        {
            JuegoMemoriaDTO juego = new JuegoMemoriaDTO();

            juego.Num1 = Random.Shared.Next(1, 51);
            juego.Num2 = Random.Shared.Next(1, 51);
            juego.Num3 = Random.Shared.Next(1, 51);
            juego.Num4 = Random.Shared.Next(1, 51);
            juego.Num5 = Random.Shared.Next(1, 51);

            string[] tiposPregunta = { "dos pares", "dos impares", "suma mas 50", "hay menor a 10", "hay dos iguales" };

            int indiceAleatorio = Random.Shared.Next(tiposPregunta.Length);
            juego.TipoPregunta = tiposPregunta[indiceAleatorio];

            switch (juego.TipoPregunta)
            {
                case "dos pares":
                    juego.Pregunta = "¿Habia exactamente 2 números pares?";
                    juego.CodigoPregunta = "2PARES";
                    break;

                case "dos impares":
                    juego.Pregunta = "¿Habia exactamente 2 números impares?";
                    juego.CodigoPregunta = "2IMPARES";
                    break;

                case "suma mas 50":
                    juego.Pregunta = "¿La suma de todos los números superaba 50?";
                    juego.CodigoPregunta = "SUMATODOMAYOR50";
                    break;

                case "hay menor a 10":
                    juego.Pregunta = "¿Había algún número menor a 10?";
                    juego.CodigoPregunta = "ALGUNO_MENOR10";
                    break;

                case "hay dos iguales":
                    juego.Pregunta = "¿Había 2 números iguales?";
                    juego.CodigoPregunta = "2IGUALES";
                    break;
            }

            return juego;
        }

        //sub clase anidada
        public class CuerpoMemoria
        {
            public int[] SecuenciaNumeros { get; set; } = Array.Empty<int>();
            public string Pregunta { get; set; } = "";
        }

        public ResultadoValidacion ValidarRespuesta(Pregunta preg, string respuestaUsuario)
        {
            ResultadoValidacion resultadoValidacion = new ResultadoValidacion();
            return resultadoValidacion;
        }
    }
}

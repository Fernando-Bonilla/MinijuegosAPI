using MinijuegosAPI.Data;
using MinijuegosAPI.DTOs;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using System.Text.Json;

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

            JuegoMatematicaDTO juego = CrearJuegoDeMatematica();

            CuerpoMatematica cuerpoPregunta = new CuerpoMatematica
            {
                SecuenciaNumeros = new int[] { juego.Num1, juego.Num2, juego.Num3},
                Pregunta = juego.Pregunta
            };

            //string json = JsonSerializer.Serialize( cuerpoPregunta, new JsonSerializerOptions());
            string json = JsonSerializer.Serialize(cuerpoPregunta, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Pregunta pregunta = new Pregunta
            {
                Tipo = "Matematica",                
                CuerpoPregunta = json,
            };

            _context.Preguntas.Add(pregunta);
            _context.SaveChanges();

            return pregunta;
        }

        public static JuegoMatematicaDTO CrearJuegoDeMatematica()
        {
            JuegoMatematicaDTO juego = new JuegoMatematicaDTO();

            juego.Num1 = Random.Shared.Next(1, 51);
            juego.Num2 = Random.Shared.Next(1, 51);
            juego.Num3 = Random.Shared.Next(1, 51);            

            string pregunta = "Cual es la suma de todos los numeros";

            juego.TipoPregunta = "Matematica";
            juego.Pregunta = pregunta;           

            return juego;
        }

        //sub clase anidada
        public class CuerpoMatematica
        {
            public int[] SecuenciaNumeros { get; set; } = Array.Empty<int>();
            public string Pregunta { get; set; } = "";
        }
    }
}


using Microsoft.EntityFrameworkCore;
using MinijuegosAPI.Data;
using MinijuegosAPI.DTOs;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinijuegosAPI.Services{
    
    public class MiniJuegoLogica : IMiniJuego
    {
        private readonly ApplicationDbContext _context;
        public MiniJuegoLogica(ApplicationDbContext context)
        {
            _context = context;
        }
        public Pregunta GenerarPregunta()
        {           

            JuegoLogicaDTO juego = CrearJuegoDeLogica();

            // aca me voy a mandar una re sarpada, voy a generar el CuerpoPregunta con solo la secuencia de numeros y la pregunta
            CuerpoLogica cuerpoPregunta = new CuerpoLogica
            {
                SecuenciaNumeros = new int[] { juego.Num1, juego.Num2, juego.Num3 },
                Pregunta = juego.Pregunta,
            };

            string json = JsonSerializer.Serialize(cuerpoPregunta, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Pregunta pregunta = new Pregunta
            {
                Tipo = "Logica",
                Codigo = juego.CodigoPregunta,
                CuerpoPregunta = json,
            };

            _context.Preguntas.Add(pregunta);
            _context.SaveChanges();             
            
            return pregunta;
        }
       
        public static JuegoLogicaDTO CrearJuegoDeLogica()
        {
            JuegoLogicaDTO juego = new JuegoLogicaDTO();

            juego.Num1 = Random.Shared.Next(1, 51);
            juego.Num2 = Random.Shared.Next(1, 51);
            juego.Num3 = Random.Shared.Next(1, 51);

            string[] tiposPregunta = { "dos pares", "suma par", "mayor que la suma", "mayor a 50", "todos diferentes" };

            int indiceAleatorio = Random.Shared.Next(tiposPregunta.Length);
            juego.TipoPregunta = tiposPregunta[indiceAleatorio];

            switch (juego.TipoPregunta)
            {
                case "dos pares":
                    juego.Pregunta = "Exactamente 2 números son pares";
                    juego.CodigoPregunta = "2PARES";
                    break;

                case "suma par":
                    juego.Pregunta = "La suma de los 3 números es par";
                    juego.CodigoPregunta = "SUMA_PAR";
                    break;

                case "mayor que la suma":
                    juego.Pregunta = "El número mayor es mayor que la suma de los otros dos";
                    juego.CodigoPregunta = "MAYOR_SUMA_OTROS";
                    break;

                case "mayor a 50":
                    juego.Pregunta = "Hay al menos un número mayor que 50";
                    juego.CodigoPregunta = "ALGUNO_MAYOR50";
                    break;

                case "todos diferentes":
                    juego.Pregunta = "Todos los números son diferentes";
                    juego.CodigoPregunta = "TODOS_DIFERENTES";
                    break;
            }

            return juego;

        }

        //sub clase anidada
        public class CuerpoLogica
        {
            [JsonPropertyName("secuenciaNumeros")]
            public int[] SecuenciaNumeros { get; set; } = Array.Empty<int>();

            [JsonPropertyName("pregunta")]
            public string Pregunta { get; set; } = "";
        }

        public ResultadoValidacion ValidarRespuesta(Pregunta preg, string respuestaUsuario) 
        {
            CuerpoLogica? cuerpoLogica = JsonSerializer.Deserialize<CuerpoLogica>(preg.CuerpoPregunta);

            int[] ints = cuerpoLogica.SecuenciaNumeros;
            string? codigoPregunta = preg.Codigo;

            respuestaUsuario = respuestaUsuario.Trim().ToLower();            
            bool respuestaUsuarioBooleada = respuestaUsuario == "si" ? true : false;

            bool respCorrecta = false;

            Console.WriteLine($"[VALIDAR] id={preg.Id} codigo={codigoPregunta} numeros=[{string.Join(",", ints)}]");

            switch (codigoPregunta)
            {
                case "2PARES":
                    respCorrecta = ExactDosNumerosPares(ints);
                    break;

                case "SUMA_PAR":
                    respCorrecta = LaSumaEsPar(ints);
                    break;

                case "MAYOR_SUMA_OTROS":
                    respCorrecta = MayorQueSumaDeLosOtros(ints);
                    break;

                case "ALGUNO_MAYOR50":
                    respCorrecta = AlMenosUnoMayorA50(ints);
                    break;

                case "TODOS_DIFERENTES":
                    respCorrecta = TodosDiferentes(ints);
                    break;
            }

            ResultadoValidacion resultadoValidacion = new ResultadoValidacion();
            resultadoValidacion.EsCorrecta = respuestaUsuarioBooleada == respCorrecta;
            resultadoValidacion.Mensaje = respuestaUsuarioBooleada == respCorrecta ? "Bien Maquina del mal, tas re sarpado" : "No viejita, le erraste, seguí intentando";
            resultadoValidacion.RespuestaCorrecta = respCorrecta == true ? "Si" : "No";

            return resultadoValidacion;
            
        }

        public static bool ExactDosNumerosPares(int[] nums)
        {
            int pares = 0;

            foreach (int num in nums)
            {
                if ((num % 2) == 0)
                {
                    pares++;
                }
            }

            return pares == 2;
        }

        public static bool LaSumaEsPar(int[] nums)
        {
            int suma = 0;

            foreach (int num in nums)
            {
                suma += num;
            }
            return ((suma % 2) == 0);
        }

        public static bool MayorQueSumaDeLosOtros(int[] nums)
        {
            if (nums[0] > nums[1] && nums[0] > nums[2])
                return (nums[0] > nums[1] + nums[2]);

            if (nums[1] > nums[0] && nums[1] > nums[2])
                return (nums[1] > nums[0] + nums[2]);

            if (nums[2] > nums[0] && nums[2] > nums[1])
                return (nums[2] > nums[0] + nums[1]);

            return false;
        }

        public static bool AlMenosUnoMayorA50(int[] nums)
        {
            if (nums[0] > 50)
                return true;

            if (nums[1] > 50)
                return true;

            if (nums[2] > 50)
                return true;

            return false;
        }

        public static bool TodosDiferentes(int[] nums)
        {
            return (nums[0] != nums[1] && nums[1] != nums[2] && nums[0] != nums[2]);
        }


    }
}

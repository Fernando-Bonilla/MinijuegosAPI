using MinijuegosAPI.Data;
using MinijuegosAPI.DTOs;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            [JsonPropertyName("secuenciaNumeros")]
            public int[] SecuenciaNumeros { get; set; } = Array.Empty<int>();
            [JsonPropertyName("pregunta")]
            public string Pregunta { get; set; } = "";
        }

        public ResultadoValidacion ValidarRespuesta(Pregunta preg, string respuestaUsuario)
        {
            CuerpoMemoria? cuerpoPregMemoria = JsonSerializer.Deserialize<CuerpoMemoria>(preg.CuerpoPregunta);

            string? codigoPregunta = preg.Codigo;
            int[] ints = cuerpoPregMemoria.SecuenciaNumeros;           

            respuestaUsuario = respuestaUsuario.Trim().ToLower();
            respuestaUsuario = respuestaUsuario == "sí" ? "si" : respuestaUsuario; // chequeo por si viene con tilde

            bool respuestaUsuarioBooleada = respuestaUsuario == "si" || respuestaUsuario == "true";

            bool respCorrecta = false;            

            switch (codigoPregunta)
            {
                case "2PARES":
                    respCorrecta = DosNumerosPares(ints);
                    break;

                case "2IMPARES":
                    respCorrecta = DosNumerosImpares(ints);
                    break;

                case "SUMATODOMAYOR50":
                    respCorrecta = SumaMayorA50(ints);
                    break;

                case "ALGUNO_MENOR10":
                    respCorrecta = AlgunMenorA10(ints);
                    break;

                case "2IGUALES":
                    respCorrecta = DosNumerosIguales(ints);
                    break;               
            }

            ResultadoValidacion resultadoValidacion = new ResultadoValidacion();
            resultadoValidacion.EsCorrecta = respuestaUsuarioBooleada == respCorrecta;
            resultadoValidacion.Mensaje = respuestaUsuarioBooleada == respCorrecta ? "Bien Maquina del mal, tas re sarpado" : "No viejita, le erraste, seguí intentando";
            resultadoValidacion.RespuestaCorrecta = respCorrecta == true ? "Si" : "No";          

            return resultadoValidacion;
        }

        public static bool SumaMayorA50(int[] numeros)
        {
            if (numeros == null || numeros.Length == 0)
            { 
                return false; // como por las dudas
            }

            int suma = 0;
            foreach (int num in numeros)
            {
                suma += num;
            }

            return suma > 50;
        }

        public static bool DosNumerosPares(int[] nums)
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

        public static bool DosNumerosIguales(int[] nums)
        {
            int iguales = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 0; j < nums.Length; j++)
                {
                    if (i != j && nums[i] == nums[j])
                    {
                        iguales++;
                    }
                }
            }
            return iguales >= 2;
        }

        public static bool AlgunMenorA10(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] < 10)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DosNumerosImpares(int[] nums)
        {
            int impares = 0;

            foreach (int num in nums)
            {
                if ((num % 2) != 0)
                {
                    impares++;
                }
            }

            return impares == 2;
        }
    }
}

using MinijuegosAPI.DTOs;
using MinijuegosAPI.Models;
using MinijuegosAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinijuegosAPI.Tests.Services
{
    public class MiniJuegoLogicaTests
    {
        [Fact]
        public void Crear_Juego_Logica_Genera_Numeros_Rango_1_Al_50()
        {
            // Act
            JuegoLogicaDTO juego = MiniJuegoLogica.CrearJuegoDeLogica();

            // Assert
            Assert.InRange(juego.Num1, 1, 50);
            Assert.InRange(juego.Num2, 1, 50);
            Assert.InRange(juego.Num3, 1, 50);
        }

        [Fact]
        public void Crear_Juego_Logica_Setea_Tipo_Pregunta_Y_Codigos()
        {
            // Act
            JuegoLogicaDTO juego = MiniJuegoLogica.CrearJuegoDeLogica();

            // Assert
            string[] tiposValidos = { "dos pares", "suma par", "mayor que la suma", "mayor a 50", "todos diferentes" };

            Assert.Contains(juego.TipoPregunta, tiposValidos);
            Assert.False(string.IsNullOrWhiteSpace(juego.Pregunta));
            Assert.False(string.IsNullOrWhiteSpace(juego.CodigoPregunta));
        }

        [Fact]
        public void Dos_Numeros_Pares_Cuando_Hay_Dos_Pares_Devuelve_True()
        {
            int[] nums = new int[] { 2, 4, 3 }; 

            bool resultado = MiniJuegoLogica.ExactDosNumerosPares(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Dos_Numeros_Pares_Cuando_No_Hay_Dos_Pares_Devuelve_False()
        {
            int[] nums = new int[] { 2, 4, 6 };

            bool resultado = MiniJuegoLogica.ExactDosNumerosPares(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Cuando_La_Suma_Es_Par_Devuelve_True()
        {
            int[] nums = new int[] { 1, 1, 2 }; 

            bool resultado = MiniJuegoLogica.LaSumaEsPar(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Cuando_La_Suma_Es_Impar_Devuelve_False()
        {
            int[] nums = new int[] { 1, 1, 1 };

            bool resultado = MiniJuegoLogica.LaSumaEsPar(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Numero_Mayor_Que_La_Suma_De_Los_Otros_Devuelve_True()
        {
            int[] nums = new int[] { 10, 3, 2 }; 

            bool resultado = MiniJuegoLogica.MayorQueSumaDeLosOtros(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Numero_Mayor_No_Mayor_Que_La_Suma_De_Los_Otros_Devuelve_False()
        {
            int[] nums = new int[] { 5, 3, 2 }; 

            bool resultado = MiniJuegoLogica.MayorQueSumaDeLosOtros(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Cuando_Mayor_Es_Repetido_La_Suma_De_Los_Otros_Devuelve_False()
        {
            int[] nums = new int[] { 5, 5, 1 }; 

            bool resultado = MiniJuegoLogica.MayorQueSumaDeLosOtros(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Al_Menos_Uno_Es_Mayor_A_50_Devuelve_True()
        {
            int[] nums = new int[] { 51, 10, 20 };

            bool resultado = MiniJuegoLogica.AlMenosUnoMayorA50(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Cuando_No_Hay_Mayor_A_50_False()
        {
            int[] nums = new int[] { 10, 20, 50 };

            bool resultado = MiniJuegoLogica.AlMenosUnoMayorA50(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Cuando_Son_Todos_Diferentes_Devuelve_True()
        {
            int[] nums = new int[] { 1, 2, 3 };

            bool resultado = MiniJuegoLogica.TodosDiferentes(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Cuando_No_Son_Todos_Diferentes_Devuelve_False()
        {
            int[] nums = new int[] { 1, 1, 2 };

            bool resultado = MiniJuegoLogica.TodosDiferentes(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Validar_Respuesta_Suma_Par_Usuario_Le_Emboca_Devuelve_True()
        {
            // Arrange
            MiniJuegoLogica.CuerpoLogica cuerpo = new MiniJuegoLogica.CuerpoLogica
            {
                SecuenciaNumeros = new int[] { 1, 1, 2 },
                Pregunta = "La suma de los 3 números es par"
            };

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Logica",
                Codigo = "SUMA_PAR",
                CuerpoPregunta = json
            };

            MiniJuegoLogica miniJuego = new MiniJuegoLogica(null!);

            string respuestaUsuario = "verdadero";

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.True(resultado.EsCorrecta);
            Assert.Equal("Verdadero", resultado.RespuestaCorrecta);
            Assert.Equal("Bien Maquina del mal, tas re sarpado", resultado.Mensaje);
        }


        [Fact]
        public void Validar_Respuesta_Suma_Par_Usuario_Se_Equivoca_Devuelve_False()
        {
            // Arrange
            MiniJuegoLogica.CuerpoLogica cuerpo = new MiniJuegoLogica.CuerpoLogica
            {
                SecuenciaNumeros = new int[] { 1, 2, 4 },
                Pregunta = "La suma de los 3 números es par"
            };            

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Logica",
                Codigo = "SUMA_PAR",
                CuerpoPregunta = json
            };

            MiniJuegoLogica miniJuego = new MiniJuegoLogica(null!);

            string respuestaUsuario = "verdadero"; 

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.False(resultado.EsCorrecta);
            Assert.Equal("Falso", resultado.RespuestaCorrecta);
            Assert.Equal("No viejita, le erraste, seguí intentando", resultado.Mensaje);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinijuegosAPI.DTOs;
using MinijuegosAPI.Models;
using MinijuegosAPI.Services;
using System.Text.Json;

namespace MinijuegosAPI.Tests.Services
{
    public class MiniJuegoMemoriaTests
    {       

        [Fact]
        public void Crear_Juego_Memoria_Genera_Numeros_Rango_1_Al_50()
        {
            JuegoMemoriaDTO juego = MiniJuegoMemoria.CrearJuegoDeMemoria();

            Assert.InRange(juego.Num1, 1, 50);
            Assert.InRange(juego.Num2, 1, 50);
            Assert.InRange(juego.Num3, 1, 50);
            Assert.InRange(juego.Num4, 1, 50);
            Assert.InRange(juego.Num5, 1, 50);
        }

        [Fact]
        public void Crear_Juego_Memoria_Setea_Tipo_Pregunta_Y_Codigos()
        {
            JuegoMemoriaDTO juego = MiniJuegoMemoria.CrearJuegoDeMemoria();

            string[] tiposValidos = { "dos pares", "dos impares", "suma mas 50", "hay menor a 10", "hay dos iguales" };

            Assert.Contains(juego.TipoPregunta, tiposValidos);
            Assert.False(string.IsNullOrWhiteSpace(juego.Pregunta));
            Assert.False(string.IsNullOrWhiteSpace(juego.CodigoPregunta));
        }

        [Fact]
        public void Suma_Mayor_A_50_Devuelve_True()
        {
            int[] numeros = { 20, 20, 20 }; 

            bool resultado = MiniJuegoMemoria.SumaMayorA50(numeros);

            Assert.True(resultado);
        }

        [Fact]
        public void Suma_Mayor_A_50_Cuando_Es_Menor_50_Devuelve_False()
        {
            int[] numeros = { 10, 10, 10 }; 

            bool resultado = MiniJuegoMemoria.SumaMayorA50(numeros);

            Assert.False(resultado);
        }

        [Fact]
        public void Suma_Mayor_A_50_Cuando_Es_Null_O_Empty_Devuelve_False()
        {
            int[] numerosVacios = Array.Empty<int>();

            bool resultado = MiniJuegoMemoria.SumaMayorA50(numerosVacios);

            Assert.False(resultado);
        }

        [Fact]
        public void Dos_Numeros_Pares_Cuando_Hay_Dos_Pares_Devuelve_True()
        {
            int[] nums = { 2, 4, 3, 5, 7 }; 

            bool resultado = MiniJuegoMemoria.DosNumerosPares(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Dos_Numeros_Pares_Cuando_No_Hay_Dos_Pares_Devuelve_False()
        {
            int[] nums = { 2, 4, 6, 7, 9 }; 

            bool resultado = MiniJuegoMemoria.DosNumerosPares(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Dos_Numeros_Impares_Cuando_Hay_DosImpares_Devuelve_True()
        {
            int[] nums = { 1, 3, 2, 4, 6 };

            bool resultado = MiniJuegoMemoria.DosNumerosImpares(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Dos_Numeros_Impares_Cuando_No_Hay_DosImpares_Devuelve_True()
        {
            int[] nums = { 1, 3, 5, 2, 4 };

            bool resultado = MiniJuegoMemoria.DosNumerosImpares(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Alguno_Menor_A_10_Cuando_Hay_Menor_A_10_Devuelve_True()
        {
            int[] nums = { 5, 20, 30, 40, 50 };

            bool resultado = MiniJuegoMemoria.AlgunMenorA10(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Alguno_Menor_A_10_Cuando_No_Hay_Menor_A_10_Devuelve_False()
        {
            int[] nums = { 10, 20, 30, 40, 50 };

            bool resultado = MiniJuegoMemoria.AlgunMenorA10(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Dos_Numeros_Iguales_Cuando_Hay_Dos_Iguales_Devuelve_True()
        {
            int[] nums = { 1, 2, 3, 2, 5 }; 

            bool resultado = MiniJuegoMemoria.DosNumerosIguales(nums);

            Assert.True(resultado);
        }

        [Fact]
        public void Dos_Numeros_Iguales_Cuando_No_Hay_Dos_Iguales_Devuelve_False()
        {
            int[] nums = { 1, 2, 3, 4, 5 };

            bool resultado = MiniJuegoMemoria.DosNumerosIguales(nums);

            Assert.False(resultado);
        }

        [Fact]
        public void Validar_Respuesta_Suma_Mayor_A_50_Respuesta_Si_Cuando_Es_Correcto_Devuelve_True()
        {
            // Arrange
            MiniJuegoMemoria.CuerpoMemoria cuerpo = new MiniJuegoMemoria.CuerpoMemoria
            {
                SecuenciaNumeros = new[] { 20, 20, 20 }, 
                Pregunta = "¿La suma de todos los números superaba 50?"
            };

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Memoria",
                Codigo = "SUMATODOMAYOR50",
                CuerpoPregunta = json
            };

            MiniJuegoMemoria miniJuego = new MiniJuegoMemoria(null!);

            string respuestaUsuario = "sí"; // testo con tilde y despues sin

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.True(resultado.EsCorrecta);
            Assert.Equal("Si", resultado.RespuestaCorrecta);
            Assert.Equal("Bien Maquina del mal, tas re sarpado", resultado.Mensaje);
        }

        [Fact]
        public void Validar_Respuesta_Suma_Mayor_A_50_Respuesta_Si_Con_Tilde_Cuando_Es_Correcto_Devuelve_True()
        {
            // Arrange
            MiniJuegoMemoria.CuerpoMemoria cuerpo = new MiniJuegoMemoria.CuerpoMemoria
            {
                SecuenciaNumeros = new[] { 20, 20, 20 },
                Pregunta = "¿La suma de todos los números superaba 50?"
            };

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Memoria",
                Codigo = "SUMATODOMAYOR50",
                CuerpoPregunta = json
            };

            MiniJuegoMemoria miniJuego = new MiniJuegoMemoria(null!);

            string respuestaUsuario = "si"; // testo con tilde y despues sin

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.True(resultado.EsCorrecta);
            Assert.Equal("Si", resultado.RespuestaCorrecta);
            Assert.Equal("Bien Maquina del mal, tas re sarpado", resultado.Mensaje);
        }

        [Fact]
        public void Validar_Respuesta_Suma_Mayor_A_50_Usuario_Se_Equivoca_Devuelve_False()
        {
            // Arrange
            MiniJuegoMemoria.CuerpoMemoria cuerpo = new MiniJuegoMemoria.CuerpoMemoria
            {
                SecuenciaNumeros = new[] { 20, 20, 20 }, 
                Pregunta = "¿La suma de todos los números superaba 50?"
            };

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Memoria",
                Codigo = "SUMATODOMAYOR50",
                CuerpoPregunta = json
            };

            MiniJuegoMemoria miniJuego = new MiniJuegoMemoria(null!);

            string respuestaUsuario = "no";

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.False(resultado.EsCorrecta);
            Assert.Equal("Si", resultado.RespuestaCorrecta);
            Assert.Equal("No viejita, le erraste, seguí intentando", resultado.Mensaje);
        }


    }

}

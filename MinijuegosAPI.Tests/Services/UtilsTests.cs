using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinijuegosAPI.Models;
using MinijuegosAPI.Services;

namespace MinijuegosAPI.Tests.Services
{
    public class UtilsTests
    {
        [Theory]
        [InlineData("5", true)]         // Número simple
        [InlineData("  10  ", true)]    // Espacios (Trim)
        [InlineData("-50", true)]       // Número negativo
        [InlineData("cinco", false)]    // Texto no numérico
        [InlineData("5.5", false)]      // Decimal (TryParse int fallará)
        public void Matematica_ValidaCorrectamente(string entrada, bool esperado)
        {
            // Arrange
            var pregunta = new Pregunta { Tipo = "Matematica" };

            // Act
            var resultado = Utils.ValidarEntradaRespuesta(pregunta, entrada);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("verdadero", true)]
        [InlineData("FALSO", true)]     // Mayúsculas (ToLower)
        [InlineData(" true ", true)]    // Espacios y mezcla
        [InlineData("false", true)]
        [InlineData("quizas", false)]   // Valor inválido
        [InlineData("verdad", false)]   // Casi, pero no exacto
        public void Logica_ValidaPalabrasClave(string entrada, bool esperado)
        {
            // Arrange
            var pregunta = new Pregunta { Tipo = "Logica" };

            // Act
            var resultado = Utils.ValidarEntradaRespuesta(pregunta, entrada);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("si", true)]
        [InlineData("sí", true)]        // Con tilde
        [InlineData("NO", true)]        // Mayúsculas
        [InlineData("true", true)]      // Incluido en tu lógica
        [InlineData("false", true)]     // Incluido en tu lógica
        [InlineData("tal vez", false)]
        public void Memoria_ValidaPalabrasClave(string entrada, bool esperado)
        {
            // Arrange
            var pregunta = new Pregunta { Tipo = "Memoria" };

            // Act
            var resultado = Utils.ValidarEntradaRespuesta(pregunta, entrada);

            // Assert
            Assert.Equal(esperado, resultado);
        }


        [Fact]
        public void TipoDesconocido_RetornaFalse()
        {
            var pregunta = new Pregunta { Tipo = "Geografia" };
            var resultado = Utils.ValidarEntradaRespuesta(pregunta, "Paris");
            Assert.False(resultado);
        }

        [Fact]
        public void RespuestaNula_RetornaFalse()
        {
            var pregunta = new Pregunta { Tipo = "Matematica" };
            // Enviamos null para asegurar que no explote por el Trim()
            // (Nota: Agregué una validación de null al inicio de tu método en este ejemplo para seguridad)
            var resultado = Utils.ValidarEntradaRespuesta(pregunta, null!);
            Assert.False(resultado);
        }


    }
}

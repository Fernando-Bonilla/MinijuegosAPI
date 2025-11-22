using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MinijuegosAPI.DTOs;
using MinijuegosAPI.Models;
using MinijuegosAPI.Services;

namespace MinijuegosAPI.Tests.Services
{
    public class UtilsTests
    {
        [Theory]
        [InlineData("5", true)]         
        [InlineData("  10  ", true)]    
        [InlineData("-50", true)]       
        [InlineData("cinco", false)]    
        [InlineData("5.5", false)]      
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
        [InlineData("FALSO", true)]    
        [InlineData(" true ", true)]    
        [InlineData("false", true)]
        [InlineData("quizas", false)]   
        [InlineData("verdad", false)]   
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
        [InlineData("sí", true)]        
        [InlineData("NO", true)]        
        [InlineData("true", true)]     
        [InlineData("false", true)]     
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
            var resultado = Utils.ValidarEntradaRespuesta(pregunta, null!);
            Assert.False(resultado);
        }

        [Fact]
        public void MapearDTO_TipoLogica_RetornaJuegoLogicaResponseDTO()
        {
            // Arrange
            var fecha = DateTime.Now;
            var jsonCuerpo = "{\"secuenciaNumeros\": [1, 2, 3], \"pregunta\": \"¿Mayor a 50?\"}";

            var pregunta = new Pregunta
            {
                Id = 1,
                Tipo = "Logica",
                CuerpoPregunta = jsonCuerpo,
                Codigo = "MAYORA50",
                FechaCreacion = fecha
            };

            // Act
            var resultado = Utils.mapearDTOcorrecto(pregunta);

            // Assert
            var dto = Assert.IsType<JuegoLogicaResponseDTO>(resultado);

            Assert.Equal(1, dto.Id);
            Assert.Equal("Logica", dto.TipoPregunta);
            Assert.Equal("MAYORA50", dto.CodigoPregunta);
            Assert.Equal("¿Mayor a 50?", dto.Pregunta);
            Assert.Equal(new int[] { 1, 2, 3 }, dto.Secuencia);
            Assert.Equal(fecha, dto.FechaCreacion);
        }

        [Fact]
        public void MapearDTO_TipoMemoria_RetornaJuegoMemoriaResponseDTO()
        {
            // Arrange
            var jsonCuerpo = "{\"secuenciaNumeros\": [9, 8, 20, 56, 89], \"pregunta\": \"¿Habia 2 exacamtne iguales?\"}";
            var pregunta = new Pregunta
            {
                Id = 2,
                Tipo = "Memoria",
                CuerpoPregunta = jsonCuerpo,
                Codigo = "2IGUALES",
                FechaCreacion = DateTime.Now
            };

            // Act
            var resultado = Utils.mapearDTOcorrecto(pregunta);

            // Assert
            var dto = Assert.IsType<JuegoMemoriaResponseDTO>(resultado);

            Assert.Equal("Memoria", dto.TipoPregunta);
            Assert.Equal("2IGUALES", dto.CodigoPregunta);
            Assert.Equal("¿Habia 2 exacamtne iguales?", dto.Pregunta);
        }

        [Fact]
        public void MapearDTO_TipoMatematica_RetornaJuegoMatematicaResponseDTO()
        {
            // Arrange
            var jsonCuerpo = "{\"secuenciaNumeros\": [10, 20, 30], \"pregunta\": \"Suma\"}";
            var pregunta = new Pregunta
            {
                Id = 3,
                Tipo = "Matematica",
                CuerpoPregunta = jsonCuerpo,
                FechaCreacion = DateTime.Now
            };

            // Act
            var resultado = Utils.mapearDTOcorrecto(pregunta);

            // Assert
            var dto = Assert.IsType<JuegoMatematicaResponseDTO>(resultado);

            Assert.Equal("Matematica", dto.TipoPregunta);
            Assert.Equal("Suma", dto.Pregunta);
            
        }

        [Fact]
        public void MapearDTO_TipoDesconocido_RetornaObjetoVacio()
        {
            // Arrange
            var pregunta = new Pregunta
            {
                Id = 99,
                Tipo = "Desconocido",
                CuerpoPregunta = "{}",
                Codigo = "ERR",
                FechaCreacion = DateTime.Now
            };

            // Act
            var resultado = Utils.mapearDTOcorrecto(pregunta);

            // Assert
            Assert.NotNull(resultado);

            Assert.False(resultado is JuegoLogicaResponseDTO);
            Assert.False(resultado is JuegoMemoriaResponseDTO);
            Assert.False(resultado is JuegoMatematicaResponseDTO);

            Assert.Equal("{ }", resultado.ToString());
        }

        [Fact]
        public void MapearDTO_JsonInvalido_LanzaExcepcion()
        {
            // Arrange
            var pregunta = new Pregunta
            {
                Tipo = "Logica",
                CuerpoPregunta = "ESTO NO ES JSON",
            };

            // Act & Assert
            Assert.Throws<JsonException>(() => Utils.mapearDTOcorrecto(pregunta));
        }
    }
}

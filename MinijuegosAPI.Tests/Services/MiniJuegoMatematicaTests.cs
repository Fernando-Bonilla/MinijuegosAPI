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
    public  class MiniJuegoMatematicaTests
    {
        [Fact]
        public void Crear_Juego_Tipo_Matematica() 
        {
            JuegoMatematicaDTO juego = MiniJuegoMatematica.CrearJuegoDeMatematica();

            // Assert
            Assert.NotNull(juego);
            Assert.Equal("Matematica", juego.TipoPregunta);
            Assert.Equal("Cual es la suma de todos los numeros", juego.Pregunta);
        }

        [Fact]
        public void Juego_Matematica_Numeros_Rango_1_Al_50()
        {
            JuegoMatematicaDTO juego = MiniJuegoMatematica.CrearJuegoDeMatematica();

            Assert.InRange(juego.Num1, 1, 50);
            Assert.InRange(juego.Num2, 1, 50);
            Assert.InRange(juego.Num3, 1, 50);
        }

        [Fact]
        public void Validar_Respuesta_Cuando_Es_Correcta_Devuelve_True()
        {
            MiniJuegoMatematica.CuerpoMatematica cuerpo = new MiniJuegoMatematica.CuerpoMatematica
            {
                SecuenciaNumeros = new[] { 1, 2, 3 },
                Pregunta = "Cual es la suma de todos los numeros"
            };

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Matematica",
                CuerpoPregunta = json
            };

            MiniJuegoMatematica miniJuego = new MiniJuegoMatematica(null!);

            string respuestaUsuario = "6"; // aca la version correcta

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.True(resultado.EsCorrecta);
            Assert.Equal("6", resultado.RespuestaCorrecta);
            Assert.Equal("Bien Maquina del mal, tas re sarpado", resultado.Mensaje);
        }

        [Fact]
        public void ValidarRespuesta_CuandoRespuesta_Es_Incorrecta_Devuelve_False()
        {
            // Arrange
            MiniJuegoMatematica.CuerpoMatematica cuerpo = new MiniJuegoMatematica.CuerpoMatematica
            {
                SecuenciaNumeros = new[] { 1, 2, 3 }, 
                Pregunta = "Cual es la suma de todos los numeros"
            };

            string json = JsonSerializer.Serialize(cuerpo);

            Pregunta preg = new Pregunta
            {
                Tipo = "Matematica",
                CuerpoPregunta = json
            };

            MiniJuegoMatematica miniJuego = new MiniJuegoMatematica(null!);

            string respuestaUsuario = "10"; // incorrectoooooooooooooooooooo

            // Act
            ResultadoValidacion resultado = miniJuego.ValidarRespuesta(preg, respuestaUsuario);

            // Assert
            Assert.False(resultado.EsCorrecta);
            Assert.Equal("6", resultado.RespuestaCorrecta);
            Assert.Equal("No viejita, le erraste, seguí intentando", resultado.Mensaje);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MinijuegosAPI.Services;
using MinijuegosAPI.Interfaces;

namespace MinijuegosAPI.Tests.Factory
{
    public class MiniJuegoFactoryTest
    {
        [Fact]
        public void GenerarMiniJuego_Con_Parametro_desconocido_Retorna_Null()
        {
            //Arrange
            MiniJuegoFactory factory = new MiniJuegoFactory(context: null!);

            //Act
            IMiniJuego? tipoJuego = factory.GenerarMiniJuego("asd");

            //Assert
            Assert.Null(tipoJuego);

        }

        [Fact]
        public void GenerarMiniJuego_Con_Parametro_Correcto_Genera_MiniJuego()
        {
            MiniJuegoFactory factory = new MiniJuegoFactory(context: null!);

            IMiniJuego? tipoJuego = factory.GenerarMiniJuego("logica");

            Assert.NotNull(tipoJuego);

        }

        [Fact]
        public void GenerarMiniJuego_Normalizando_ElParametro()
        {
            MiniJuegoFactory factory = new MiniJuegoFactory(context: null!);

            IMiniJuego? tipoMinijuego = factory.GenerarMiniJuego("  logica  ");

            Assert.NotNull(tipoMinijuego);
        }

        [Theory]
        [InlineData("logica", typeof(MiniJuegoLogica))]
        [InlineData("matematica", typeof(MiniJuegoMatematica))]
        [InlineData("memoria", typeof(MiniJuegoMemoria))]        
        public void GenerarMiniJuego_Correcto_Por_Tipo(string tipo, Type tipoMiniJuego )
        {
            MiniJuegoFactory factory = new MiniJuegoFactory(context: null!);

            IMiniJuego? tipoJuego = factory.GenerarMiniJuego(tipo);
            
            Assert.IsType(tipoMiniJuego, tipoJuego);
        }
    }
    
}

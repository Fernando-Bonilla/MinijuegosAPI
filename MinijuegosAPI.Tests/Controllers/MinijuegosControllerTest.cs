using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinijuegosAPI.Controllers;
using MinijuegosAPI.Data;
using MinijuegosAPI.Interfaces;
using MinijuegosAPI.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinijuegosAPI.Tests.Controllers
{
    public  class MinijuegosControllerTest
    {
        [Fact]
        public void Pregunta_Sin_Tipo_Devuelve_BadRequest()
        {
            // Arrange
            // mockeo del DbContext
            DbContextOptions<ApplicationDbContext> options =
                new DbContextOptions<ApplicationDbContext>();

            Mock<ApplicationDbContext> dBcontextMock =
                new Mock<ApplicationDbContext>(options);

            // mockeo del factory
            Mock<IMiniJuegoFactory> factoryMock =
                new Mock<IMiniJuegoFactory>();

            MinijuegosController controller =
                new MinijuegosController(dBcontextMock.Object, factoryMock.Object);

            // Act
            ActionResult<object> result = controller.pregunta(null);

            // Assert
            BadRequestObjectResult badResult =
                Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        public void Pregunta_Tipo_Invalido_Devuelve_BadRequest()
        {
            // Arrange
            DbContextOptions<ApplicationDbContext> options =
                new DbContextOptions<ApplicationDbContext>();

            Mock<ApplicationDbContext> contextMock =
                new Mock<ApplicationDbContext>(options);

            Mock<IMiniJuegoFactory> factoryMock =
                new Mock<IMiniJuegoFactory>();

            factoryMock
                .Setup(f => f.GenerarMiniJuego("lallalala"))
                .Returns((IMiniJuego)null!);

            MinijuegosController controller =
                new MinijuegosController(contextMock.Object, factoryMock.Object);

            // Act
            ActionResult<object> result = controller.pregunta("lallalala");

            // Assert
            BadRequestObjectResult badResult =
                Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Pregunta_GenerarPregunta_Devuelve_Null_Devuelve_Status_Code_500()
        {
            // Arrange
            DbContextOptions<ApplicationDbContext> options =
                new DbContextOptions<ApplicationDbContext>();

            Mock<ApplicationDbContext> contextMock =
                new Mock<ApplicationDbContext>(options);

            Mock<IMiniJuegoFactory> factoryMock =
                new Mock<IMiniJuegoFactory>();

            Mock<IMiniJuego> juegoMock = new Mock<IMiniJuego>();
            juegoMock
                .Setup(j => j.GenerarPregunta())
                .Returns((Pregunta)null!);

            factoryMock
                .Setup(f => f.GenerarMiniJuego("Logica"))
                .Returns(juegoMock.Object);

            MinijuegosController controller =
                new MinijuegosController(contextMock.Object, factoryMock.Object);

            // Act
            ActionResult<object> result = controller.pregunta("Logica");

            // Assert
            ObjectResult errorResult =
                Assert.IsType<ObjectResult>(result.Result);

            Assert.Equal(500, errorResult.StatusCode);
        }



    }
}

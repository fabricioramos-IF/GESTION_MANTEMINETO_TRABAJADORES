using Microsoft.AspNetCore.Mvc;
using Moq;
using Trabajadores.Web.Controllers;
using Trabajadores.Web.Models;
using Trabajadores.Web.Services;
using Xunit;

namespace Trabajadores.Web.Tests
{
    public class TrabajadorControllerTests
    {
        private readonly Mock<ITrabajadorService> _mockService;
        private readonly Mock<IFileUploadService> _mockFileService;
        private readonly TrabajadorController _controller;

        public TrabajadorControllerTests()
        {
            _mockService = new Mock<ITrabajadorService>();
            _mockFileService = new Mock<IFileUploadService>();
            _controller = new TrabajadorController(_mockService.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task Index_RetornaListaDeTrabajadores()
        {
            var trabajadores = new List<Trabajador>
            {
                new() { IdTrabajador = 1, Nombres = "Juan", Apellidos = "Perez", Sexo = "M", TipoDocumento = "DNI", NumeroDocumento = "123", FechaNacimiento = DateTime.Today.AddYears(-25) }
            };
            _mockService.Setup(s => s.ListarTrabajadores(null)).ReturnsAsync(trabajadores);

            var resultado = await _controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(resultado);
            var model = Assert.IsAssignableFrom<List<Trabajador>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Obtener_IdNoExistente_RetornaNotFound()
        {
            _mockService.Setup(s => s.ObtenerTrabajadorPorId(999)).ReturnsAsync((Trabajador?)null);

            var resultado = await _controller.Obtener(999);

            Assert.IsType<NotFoundObjectResult>(resultado);
        }
    }
}

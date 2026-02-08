using System.ComponentModel.DataAnnotations;
using Trabajadores.Web.Validators;
using Xunit;

namespace Trabajadores.Web.Tests
{
    public class EdadMinimaAttributeTests
    {
        private readonly EdadMinimaAttribute _validator = new(18);

        [Fact]
        public void Validar_EdadMayor18_RetornaExito()
        {
            var fecha = DateTime.Today.AddYears(-25);
            var resultado = _validator.GetValidationResult(fecha, new ValidationContext(new object()));
            Assert.Equal(ValidationResult.Success, resultado);
        }

        [Fact]
        public void Validar_EdadMenor18_RetornaError()
        {
            var fecha = DateTime.Today.AddYears(-17);
            var resultado = _validator.GetValidationResult(fecha, new ValidationContext(new object()));
            Assert.NotEqual(ValidationResult.Success, resultado);
        }
    }
}

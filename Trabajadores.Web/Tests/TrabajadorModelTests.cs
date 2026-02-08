using System.ComponentModel.DataAnnotations;
using Trabajadores.Web.Models;
using Xunit;

namespace Trabajadores.Web.Tests
{
    public class TrabajadorModelTests
    {
        [Fact]
        public void Trabajador_DatosCompletos_ValidacionExitosa()
        {
            var trabajador = new Trabajador
            {
                Nombres = "Juan",
                Apellidos = "Perez",
                TipoDocumento = "DNI",
                NumeroDocumento = "12345678",
                Sexo = "M",
                FechaNacimiento = DateTime.Today.AddYears(-25)
            };

            var context = new ValidationContext(trabajador);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(trabajador, context, results, true);

            Assert.True(isValid);
        }

        [Fact]
        public void Trabajador_SexoInvalido_ValidacionFalla()
        {
            var trabajador = new Trabajador
            {
                Nombres = "Juan",
                Apellidos = "Perez",
                TipoDocumento = "DNI",
                NumeroDocumento = "12345678",
                Sexo = "X",
                FechaNacimiento = DateTime.Today.AddYears(-25)
            };

            var context = new ValidationContext(trabajador) { MemberName = "Sexo" };
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateProperty(trabajador.Sexo, context, results);

            Assert.False(isValid);
        }
    }
}

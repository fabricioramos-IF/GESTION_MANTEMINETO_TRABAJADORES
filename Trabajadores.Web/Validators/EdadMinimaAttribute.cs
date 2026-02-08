using System.ComponentModel.DataAnnotations;

namespace Trabajadores.Web.Validators
{
    public class EdadMinimaAttribute : ValidationAttribute
    {
        private readonly int _edadMinima;

        public EdadMinimaAttribute(int edadMinima)
        {
            _edadMinima = edadMinima;
            ErrorMessage = $"Debe tener al menos {edadMinima} anios de edad";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime fechaNacimiento)
            {
                if (fechaNacimiento == DateTime.MinValue || fechaNacimiento.Year < 1900)
                    return new ValidationResult("La fecha de nacimiento no es valida");

                var hoy = DateTime.Today;

                if (fechaNacimiento > hoy)
                    return new ValidationResult("La fecha de nacimiento no puede ser futura");

                var edad = hoy.Year - fechaNacimiento.Year;
                if (fechaNacimiento.Date > hoy.AddYears(-edad))
                    edad--;

                if (edad < _edadMinima)
                    return new ValidationResult($"El trabajador debe tener al menos {_edadMinima} anios. Edad actual: {edad}");

                if (edad > 120)
                    return new ValidationResult("La fecha de nacimiento no es valida");
            }

            return ValidationResult.Success;
        }
    }
}

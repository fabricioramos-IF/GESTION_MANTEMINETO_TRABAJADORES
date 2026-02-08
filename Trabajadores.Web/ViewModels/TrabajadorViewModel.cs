using System.ComponentModel.DataAnnotations;
using Trabajadores.Web.Validators;

namespace Trabajadores.Web.ViewModels
{
    public class TrabajadorViewModel
    {
        public int IdTrabajador { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden exceder 100 caracteres")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [StringLength(20, ErrorMessage = "El tipo de documento no puede exceder 20 caracteres")]
        [Display(Name = "Tipo de Documento")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El numero de documento es obligatorio")]
        [StringLength(20, ErrorMessage = "El numero de documento no puede exceder 20 caracteres")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El numero de documento solo debe contener numeros")]
        [Display(Name = "Numero de Documento")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El sexo es obligatorio")]
        [RegularExpression("^[MF]$", ErrorMessage = "El sexo debe ser M o F")]
        public string Sexo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        [EdadMinima(18, ErrorMessage = "El trabajador debe tener 18 anios o mas")]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Fotografia")]
        public IFormFile? FotoArchivo { get; set; }

        public string? FotoUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "Direccion")]
        public string? Direccion { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trabajadores.Web.Models
{
    [Table("Trabajador")]
    public class Trabajador
    {
        [Key]
        public int IdTrabajador { get; set; }

        [Required(ErrorMessage = "Ingresa el nombre")]
        [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa los apellidos")]
        [StringLength(100, ErrorMessage = "Los apellidos no deben superar los 100 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona el tipo de documento")]
        [StringLength(20, ErrorMessage = "El tipo de documento es demasiado largo")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa el número de documento")]
        [StringLength(20, ErrorMessage = "El número de documento no debe superar los 20 caracteres")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona el sexo")]
        [RegularExpression("^[MF]$", ErrorMessage = "Selecciona una opción válida")]
        [StringLength(1)]
        public string Sexo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona la fecha de nacimiento")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(255, ErrorMessage = "La ruta de la foto es demasiado larga")]
        public string? Foto { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no debe superar los 200 caracteres")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }
    }
}
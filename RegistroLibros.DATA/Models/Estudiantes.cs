using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace RegistroLibros.DATA.Models;

public class Estudiantes
    {
    [Key]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Direccion { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime FechaNacimiento { get; set; }

    [JsonIgnore]
    public virtual ICollection<PrestamoLibros> Prestamos { get; set; } = new List<PrestamoLibros>();
}

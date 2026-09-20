namespace RegistrodeLibros.Models;

using System.ComponentModel.DataAnnotations;
public class Estudiante
    {
    [Key]
    public int EstudianteId { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Direccion { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Email{ get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateOnly FechaNacimiento { get; set; }
}


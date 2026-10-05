using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrodeLibros.Models;
public class PrestamoLibros
{
    [Key]
    public int PrestamosId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante valido")]
    public int EstudianteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro valido")]
    public int LibrosId { get; set; }

    [ForeignKey("EstudianteId")]
    public virtual Estudiantes estudiante { get; set; } = null!;

    [ForeignKey("LibrosId")]
    public virtual Libros libros { get; set; } = null!;
}

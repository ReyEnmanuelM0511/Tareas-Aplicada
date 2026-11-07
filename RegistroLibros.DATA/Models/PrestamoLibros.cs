using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.DATA.Models;
public class PrestamoLibros
{
    [Key]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime Fecha { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante valido")]
    public int EstudianteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro valido")]
    public int LibroId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public bool Devuelto { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public virtual ICollection<Devoluciones> Devoluciones { get; set; } = new List<Devoluciones>();

    [ForeignKey("EstudianteId")]
    public virtual Estudiantes Estudiante { get; set; } = null!;

    [ForeignKey("LibrosId")]
    public virtual Libros Libro { get; set; } = null!;
}

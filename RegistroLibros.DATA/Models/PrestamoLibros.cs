using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.DATA.Models;
public class PrestamoLibros
{
    public int PrestamoId { get; set; }

    public DateTime Fecha { get; set; }

    public int EstudianteId { get; set; }

    public int LibroId { get; set; }

    public bool Devuelto { get; set; }

    public virtual ICollection<Devoluciones> Devoluciones { get; set; } = new List<Devoluciones>();

    public virtual Estudiantes Estudiante { get; set; } = null!;

    public virtual Libros Libro { get; set; } = null!;
}

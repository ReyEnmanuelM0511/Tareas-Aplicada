using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.DATA.Models;

public class Libros
{
    [Key]
    public int LibroID { get; set;}

    [Required(ErrorMessage = "El campo de Titulo es Obligatorio")]
    public string? Titulo { get; set; } = null!;

    [Required(ErrorMessage = "El campo de Autor es Requerido")]
    public string? Autor { get; set; } = null!;

    [Required(ErrorMessage ="El campo de AnoPublicacion es obligatorio")]
    public int AnoPublicacion {get; set;}

    public bool Disponible { get; set; } = true;

    public int CantidadLibros { get; set; }

    public virtual ICollection<EntradaLibrosDetalle> EntradasDetalles { get; set; } = new List<EntradaLibrosDetalle>();

    public virtual ICollection<PrestamoLibros> Prestamos { get; set; } = new List<PrestamoLibros>();
}

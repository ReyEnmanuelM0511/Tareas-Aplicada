using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistrodeLibros.Models;

public class EntradaLibrosDetalle
{
    [Key]
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingrese un Id valido")]
    public int LibroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingrese un Id valido")]
    public int EntradaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingrese una Cantidad valido")]
    public int Cantidad { get; set; }

    [ForeignKey("LibroID")]
    public virtual Libros Libro { get; set; } = null!;

    [ForeignKey("EntradaId")]
    [InverseProperty("EntradaLibrosDetalle")]
    public virtual EntradaLibros EntradaLibro { get; set; } = null!;
}

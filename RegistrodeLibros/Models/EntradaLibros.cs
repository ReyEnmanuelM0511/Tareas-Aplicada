using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RegistrodeLibros.Models;

public class EntradaLibros
{
    [Key]
    public int EntradaId { get; set; }

    [Required(ErrorMessage = "Elija una fecha valida")]
    public DateOnly Fecha { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string? Concepto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe introducir una cantidad valida")]
    public int Total { get; set; }

    [InverseProperty("EntradaLibro")]
    public virtual ICollection<EntradaLibrosDetalle> EntradaLibrosDetalle { get; set; } = new List<EntradaLibrosDetalle>();

}


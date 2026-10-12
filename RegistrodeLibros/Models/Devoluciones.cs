using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistrodeLibros.Models;

namespace RegistrodeLibros.Models;

public class Devoluciones
{
    [Key]
    public int DevolucionId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un préstamo")]
    public int PrestamoId { get; set; }

    [ForeignKey("PrestamoId")]
    public virtual PrestamoLibros? Prestamo { get; set; }
}

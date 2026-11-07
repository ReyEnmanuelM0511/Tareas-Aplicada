using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistroLibros.DATA.Models;

namespace RegistroLibros.DATA.Models;

public class Devoluciones
{
    [Key]
    public int DevolucionId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime Fecha { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un préstamo")]
    public int PrestamoId { get; set; }

    [ForeignKey("PrestamoId")]
    public virtual PrestamoLibros Prestamos { get; set; } = null!;
}

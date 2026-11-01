using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistroLibros.DATA.Models;

namespace RegistroLibros.DATA.Models;

public class Devoluciones
{
    public int DevolucionId { get; set; }

    public DateTime Fecha { get; set; }

    public int PrestamoId { get; set; }

    public virtual PrestamoLibros Prestamos { get; set; } = null!;
}

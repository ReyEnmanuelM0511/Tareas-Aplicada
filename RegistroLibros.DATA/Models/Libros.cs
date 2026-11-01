using System;
using System.Collections.Generic;

namespace RegistroLibros.DATA.Models;

public partial class Libros
{
    public int LibroId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public int AnoPublicacion { get; set; }

    public int CantidadPrestamos { get; set; }

    public int CantidadDisponible { get; set; }

    public virtual ICollection<EntradaLibrosDetalle> EntradasDetalles { get; set; } = new List<EntradaLibrosDetalle>();

    public virtual ICollection<PrestamoLibros> Prestamos { get; set; } = new List<PrestamoLibros>();
}
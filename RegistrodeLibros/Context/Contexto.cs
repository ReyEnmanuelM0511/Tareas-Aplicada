namespace RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
public class Contexto : DbContext
{   
    public Contexto(DbContextOptions<Contexto> options) : base(options) {}
    public DbSet<Libros> Libros {get; set;}
    public DbSet<Estudiantes> Estudaintes {get; set; }

    public DbSet<PrestamoLibros> Prestamos { get; set; }
}

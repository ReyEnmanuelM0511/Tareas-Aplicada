namespace RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
public class Contexto : DbContext
{   
    public Contexto(DbContextOptions<Contexto> options) : base(options) {}
    public DbSet<Libros> libros {get; set;}
    public DbSet<Estudiantes> estudiantes {get; set; }
    public DbSet<PrestamoLibros> Prestamos { get; set; }
}
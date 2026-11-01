using Microsoft.EntityFrameworkCore;
using RegistroLibros.DATA.Models;
using System;
using System.Collections.Generic;

namespace RegistroLibro.DATA.Context;

public partial class Contexto : DbContext
{
    public Contexto()
    {
    }

    public Contexto(DbContextOptions<Contexto> options)
        : base(options)
    {
    }

    public virtual DbSet<Devoluciones> Devoluciones { get; set; }

    public virtual DbSet<EntradaLibros> Entradas { get; set; }

    public virtual DbSet<EntradaLibrosDetalle> EntradasDetalles { get; set; }

    public virtual DbSet<Estudiantes> Estudiantes { get; set; }

    public virtual DbSet<Libros> Libros { get; set; }

    public virtual DbSet<PrestamoLibros> Prestamos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Devoluciones>(entity =>
        {
            entity.HasKey(e => e.DevolucionId);

            entity.HasIndex(e => e.PrestamoId, "IX_Devoluciones_PrestamoId");

            entity.HasOne(d => d.Prestamos).WithMany(p => p.Devoluciones).HasForeignKey(d => d.PrestamoId);
        });

        modelBuilder.Entity<EntradaLibrosDetalle>(entity =>
        {
            entity.ToTable("EntradasDetalle");

            entity.HasIndex(e => e.EntradaId, "IX_EntradasDetalle_EntradaId");

            entity.HasIndex(e => e.LibroId, "IX_EntradasDetalle_LibroId");

            entity.HasOne(d => d.EntradaLibro).WithMany(p => p.EntradaLibrosDetalle).HasForeignKey(d => d.EntradaId);

            entity.HasOne(d => d.Libro).WithMany(p => p.EntradasDetalles).HasForeignKey(d => d.LibroId);
        });

        modelBuilder.Entity<Estudiantes>(entity =>
        {
            entity.Property(e => e.Nombres).HasColumnName("nombre");
        });

        modelBuilder.Entity<Libros>(entity =>
        {
            entity.Property(e => e.AnoPublicacion).HasColumnName("anoPublicacion");
        });

        modelBuilder.Entity<PrestamoLibros>(entity =>
        {
            entity.HasIndex(e => e.EstudianteId, "IX_Prestamos_EstudianteId");

            entity.HasIndex(e => e.LibroId, "IX_Prestamos_LibroId");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Prestamos).HasForeignKey(d => d.EstudianteId);

            entity.HasOne(d => d.Libro).WithMany(p => p.Prestamos).HasForeignKey(d => d.LibroId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
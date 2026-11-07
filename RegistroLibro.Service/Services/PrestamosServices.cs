using Aplicada1.Core;
using RegistroLibros.DATA.Models;
using RegistroLibros.DATA.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RegistroLibro.Services;

public class PrestamosServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<PrestamoLibros, int>
{
    public async Task<Result> Guardar(PrestamoLibros prestamo)
    {
        if (!await Existe(prestamo.PrestamoId))
        {
            var inserto = await Insertar(prestamo);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(prestamo);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int prestamosId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AnyAsync(p => p.PrestamoId == prestamosId);
    }

    private async Task<bool> Insertar(PrestamoLibros prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroID == prestamo.LibroId);

        if (libro == null || !libro.Disponible)
        {
            return false;
        }

        libro.Disponible = false;
        contexto.Libros.Update(libro);
        contexto.Prestamos.Add(prestamo);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(PrestamoLibros prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamoAnterior = await contexto.Prestamos.FirstOrDefaultAsync(p => p.PrestamoId == prestamo.PrestamoId);
        if (prestamoAnterior == null)
        {
            return false;
        }

        if (prestamoAnterior.LibroId != prestamo.LibroId)
        {
            var libroNuevo = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroID == prestamo.LibroId);

            if (libroNuevo == null || !libroNuevo.Disponible)
            {
                return false;
            }

            var libroAnterior = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroID == prestamoAnterior.LibroId);
            if (libroAnterior != null)
            {
                libroAnterior.Disponible = true;
            }

            libroNuevo.Disponible = false;
        }

        prestamoAnterior.EstudianteId = prestamo.EstudianteId;
        prestamoAnterior.LibroId = prestamo.LibroId;
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<PrestamoLibros?>> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(e => e.Estudiante).Include(l => l.Libro).FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<Result<List<PrestamoLibros>>> GetList(Expression<Func<PrestamoLibros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(e => e.Estudiante).Include(l => l.Libro).Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<Result> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var prestamo = await contexto.Prestamos.FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
        if (prestamo == null)
        {
            return Result.Failure(new Error("Prestamo vacio", "El prestamo debe contener valores"));
        }
        var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroID == prestamo.LibroId);
        if (libro != null)
        {
            libro.Disponible = true;
            contexto.Libros.Update(libro);
            await contexto.SaveChangesAsync();
        }
        var eliminado = await contexto.Prestamos.Where(p => p.PrestamoId == prestamoId).ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}

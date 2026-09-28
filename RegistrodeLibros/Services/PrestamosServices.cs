namespace RegistrodeLibros.Services;

using RegistrodeLibros.Models;
using RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Aplicada1.Core;

public class PrestamosServices(IDbContextFactory<Contexto> contextFactory) : IService<PrestamoLibros, int>
{
    private async Task<bool> Existe(int prestamosId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.Prestamos.AnyAsync(p => p.PrestamosId == prestamosId);
    }

    private async Task<bool> Insertar(PrestamoLibros prestamolibros)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var libro = await contexto.libros.FirstOrDefaultAsync(l => l.LibroID == prestamolibros.LibrosId);

        if(libro == null || !libro.Disponible)
        {
            return false;
        }

        libro.Disponible = false;
        contexto.libros.Update(libro);

        contexto.Prestamos.Add(prestamolibros);
        return await contexto.SaveChangesAsync() > 0;

        
    }

    private async Task<bool> Modificar(PrestamoLibros prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Prestamos.Update(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar (PrestamoLibros prestamo)
    {
        if (!await Existe(prestamo.PrestamosId))
        {
            return await Insertar(prestamo);
        }
        else
        {
            return await Modificar(prestamo);
        }
    }

    public async Task<PrestamoLibros?> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(e => e.estudiante).Include(l => l.libros).FirstOrDefaultAsync(p => p.PrestamosId == prestamoId);
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos.FirstOrDefaultAsync(p => p.PrestamosId == prestamoId);

        if(prestamo == null)
        {
            return false;
        }
        
        if(prestamo != null)
        {
            var libro = await contexto.libros.FirstOrDefaultAsync(l => l.LibroID == prestamo.LibrosId); 

            if(libro != null)
            {
                libro.Disponible = true;
                contexto.libros.Update(libro);
                await contexto.SaveChangesAsync();
            }
        }

        return await contexto.Prestamos.Where(p => p.PrestamosId == prestamoId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<PrestamoLibros>> GetList(Expression<Func<PrestamoLibros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(e => e.estudiante).Include(l => l.libros).Where(criterio).AsNoTracking().ToListAsync();
    }

    Task<bool> IService<PrestamoLibros, int>.Guardar(PrestamoLibros entidad)
    {
        return Guardar(entidad);
    }
}
    


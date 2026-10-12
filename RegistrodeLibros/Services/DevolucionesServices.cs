using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class DevolucionesServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Devoluciones, int>
{
    public async Task<Result> Guardar(Devoluciones devolucion)
    {
        if (!await Existe(devolucion.DevolucionId))
        {
            var inserto = await Insertar(devolucion);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(devolucion);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int? DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .AnyAsync(d => d.DevolucionId == DevolucionId);
    }

    private async Task<bool> Insertar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos
            .Include(p => p.libros)
            .FirstOrDefaultAsync(p => p.PrestamosId == devolucion.PrestamoId);

        if (prestamo == null || prestamo.libros == null)
        {
            return false;
        }

        prestamo.libros.Disponible = true;

        contexto.Devoluciones.Add(devolucion);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Devoluciones devolucion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var devolucionAnterior = await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == devolucion.DevolucionId);

        if (devolucionAnterior == null)
        {
            return false;
        }

        if (devolucionAnterior.PrestamoId != devolucion.PrestamoId)
        {
            var prestamoNuevo = await contexto.Prestamos
                .Include(p => p.libros)
                .FirstOrDefaultAsync(p => p.PrestamosId == devolucion.PrestamoId);

            if (prestamoNuevo == null || prestamoNuevo.libros == null)
            {
                return false;
            }

            bool yaDevuelto = await contexto.Devoluciones
                .AnyAsync(d => d.PrestamoId == devolucion.PrestamoId);

            if (yaDevuelto)
            {
                return false;
            }

            var prestamoAnterior = await contexto.Prestamos
                .Include(p => p.libros)
                .FirstOrDefaultAsync(
                    p => p.PrestamosId == devolucionAnterior.PrestamoId);

            if (prestamoAnterior?.libros != null)
            {
                prestamoAnterior.libros.Disponible = false;
            }
            prestamoNuevo.libros.Disponible = true;
        }
        devolucionAnterior.PrestamoId = devolucion.PrestamoId;
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Devoluciones?>> Buscar(int DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
            .FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);
    }

    public async Task<Result<List<Devoluciones>>> GetList(Expression<Func<Devoluciones, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Devoluciones
        .Include(d => d.Prestamo)
            .ThenInclude(p => p.estudiante)
        .Include(d => d.Prestamo)
            .ThenInclude(p => p.libros)
        .Where(criterio)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Result> Eliminar(int DevolucionId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var devolucion = await contexto.Devoluciones.FirstOrDefaultAsync(d => d.DevolucionId == DevolucionId);

        if (devolucion == null)
        {
            return Result.Failure(new Error("Devolucion vacia", "La devolucion debe contener valores"));
        }
        else
        {
            devolucion.Prestamo.libros.Disponible = false;
            contexto.Libros.Update(devolucion.Prestamo.libros);
            await contexto.SaveChangesAsync();
        }

        var elimino = await contexto.Devoluciones
            .Where(d => d.DevolucionId == DevolucionId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(elimino);
    }
}

using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Context;
using RegistrodeLibros.Models;
using Aplicada1.Core;
using System.Linq.Expressions;

namespace RegistrodeLibros.Services;

public class EntradaLibrosServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<EntradaLibros, int>
{
    public async Task<Result> Guardar(EntradaLibros Entrada)
    {
        if (!await Existe(Entrada.EntradaId))
        {
            var Inserto = Insertar(Entrada);
            return Result.Success(Inserto);
        }
        else
        {
            var modifica = Modificar(Entrada);
            return Result.Success(modifica);
        }
    }

    public async Task<bool> Existe(int EntradaLibrosId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.EntradaLibros.AnyAsync(e => e.EntradaId == EntradaLibrosId);
    }

    public async Task<bool> Insertar(EntradaLibros Entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        foreach (var detalle in Entrada.EntradaLibrosDetalle)
        {
            detalle.Libro = null;
        }
        contexto.Add(Entrada);
        await AfectarLibros(Entrada.EntradaLibrosDetalle.ToArray(), Operacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task AfectarLibros(EntradaLibrosDetalle[] detalle, Operacion operacion)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        foreach(var item in detalle)
        {
            var libro = await contexto.Libros.SingleAsync(l => l.LibroID == item.LibroId);
            if (operacion == Operacion.Suma)
                libro.CantidadLibros += item.Cantidad;
            else
                libro.CantidadLibros -= item.Cantidad;
        }
        await contexto.SaveChangesAsync();
    }

    public async Task<bool> Modificar(EntradaLibros Entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var entradaAnterior = await contexto.EntradaLibros.Include(e => e.EntradaLibrosDetalle).AsNoTracking().FirstOrDefaultAsync(e => e.EntradaId == Entrada.EntradaId);

        if(entradaAnterior != null)
        {
            await AfectarLibros(entradaAnterior.EntradaLibrosDetalle.ToArray(), Operacion.Resta);
        }

        await AfectarLibros(Entrada.EntradaLibrosDetalle.ToArray(), Operacion.Suma);
        await contexto.EntradaLibrosDetalle.Where(e => e.EntradaId == Entrada.EntradaId).ExecuteDeleteAsync();

        foreach(var detalle in Entrada.EntradaLibrosDetalle)
        {
            detalle.Libro = null;
            detalle.Id = 0;
        }

        contexto.Update(Entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<EntradaLibros?>> Buscar(int Entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.EntradaLibros.Include(e => e.EntradaLibrosDetalle).ThenInclude(d => d.Libro).FirstOrDefaultAsync(e => e.EntradaId == Entrada);
    }

    public async Task<Result<List<EntradaLibros>>> GetList(Expression<Func<EntradaLibros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.EntradaLibros.Include(e => e.EntradaLibrosDetalle).ThenInclude(d => d.Libro).Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<Result> Eliminar(int EntradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var entrada = await contexto.EntradaLibros.Include(e => e.EntradaLibrosDetalle).FirstOrDefaultAsync(e => e.EntradaId == EntradaId);

        if (entrada == null)
            return Result.Failure(new Error("Entrada.NoEncontrada", "La Entrada no fue encontrada."));

        await AfectarLibros(entrada.EntradaLibrosDetalle.ToArray(), Operacion.Resta);

        contexto.EntradaLibrosDetalle.RemoveRange(entrada.EntradaLibrosDetalle);
        contexto.EntradaLibros.Remove(entrada);
        var cantidad = await contexto.SaveChangesAsync();
        return Result.Success(entrada);
    }

    public async Task<List<EntradaLibros>> Listar(Expression<Func<EntradaLibros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.EntradaLibros.Include(e => e.EntradaLibrosDetalle).ThenInclude(d => d.Libro).Where(criterio).AsNoTracking().ToListAsync();
    }

    public enum Operacion
    {
        Suma = 1,
        Resta = 2
    }
}

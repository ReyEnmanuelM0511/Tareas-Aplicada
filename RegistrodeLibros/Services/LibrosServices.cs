using Aplicada1.Core;
using RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
using System.Linq.Expressions;

namespace RegistrodeLibros.Services;

public class LibrosServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Libros, int>
{
    public async Task<Result> Guardar(Libros libro)
    {
        if (!await Existe(libro.LibroID))
        {
            var inserto = await Insertar(libro);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(libro);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.AnyAsync(l => l.LibroID == id);
    }

    private async Task<bool> Insertar(Libros libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Libros libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Libros?>> Buscar(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.FirstOrDefaultAsync(l => l.LibroID == id);
    }

    public async Task<Result<List<Libros>>> GetList(Expression<Func<Libros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<Result> Eliminar(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Libros.Where(l => l.LibroID == id).ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}

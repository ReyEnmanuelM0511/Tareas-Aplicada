using Aplicada1.Core;
using RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
using System.Linq.Expressions;


namespace RegistrodeLibros.Services;

public class LibrosServices(IDbContextFactory<Contexto> contextFactory) : IService<Libros, int>
{
    public async Task<bool> Guardar(Libros librito)
    {
        if (!await Existe(librito.LibroID))
        {
            return await Insertar(librito);
        }
        else
        {
            return await Modificar(librito);
        }
    }

    public async Task<bool> Insertar(Libros libro)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.libros.Add(libro);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Libros libros)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.Update(libros);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Libros> Buscar(int id)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.libros.FirstOrDefaultAsync(l => l.LibroID == id);
    }

    public async Task<bool> Eliminar(int id)
    {

        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.libros.Where(l => l.LibroID == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Libros>> GetList(Expression<Func<Libros, bool>> criterio)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.libros.Where(criterio).AsNoTracking().ToListAsync();
    }

    private async Task<bool> Existe(int? id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.libros
            .AnyAsync(e => e.LibroID == id);
    }
}

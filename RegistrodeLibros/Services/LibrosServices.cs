using Aplicada1.Core;
using RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
using System.Linq.Expressions;


namespace RegistrodeLibros.Services;

public class LibrosServices(IDbContextFactory<Contexto> contextFactory) : IService<Libros, int>
{
    public async Task<bool> Guardar(Libros Libro)
    {
        if (!await Existe(Libro.LibroID))
        {
            return await Insertar(Libro);
        }
        else
        {
            return await Modificar(Libro);
        }
    }

    public async Task<bool> Insertar(Libros Libro)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.Libros.Add(Libro);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Libros Libro)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.Update(Libro);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Libros> Buscar(int id)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.Libros.FirstOrDefaultAsync(l => l.LibroID == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.Libros.Where(l => l.LibroID == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Libros>> GetList(Expression<Func<Libros, bool>> criterio)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.Libros.Where(criterio).AsNoTracking().ToListAsync();
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(e => e.LibroID == id);
    }
}

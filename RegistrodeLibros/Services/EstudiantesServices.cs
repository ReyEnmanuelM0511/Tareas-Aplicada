namespace RegistrodeLibros.Services;

using Aplicada1.Core;
using Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
using System.Linq.Expressions;

public class EstudiantesServices(IDbContextFactory<Contexto> contextFactory) : IService<Estudiantes, int>
{

    public async Task<bool> Guardar(Estudiantes estudiante)
    {
        if (!await Existe(estudiante.EstudianteId))
        {
            return await Insertar(estudiante);
        }
        else
        {
            return await Modificar(estudiante);
        }
    }

    public async Task<bool> Insertar(Estudiantes estudiante)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.Estudiantes.Add(estudiante);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Estudiantes estudiante)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.Update(estudiante);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<Estudiantes> Buscar(int id)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.Estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == id);
    }

    public async Task<bool> Eliminar(int id)
    {

        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.Estudiantes.Where(e => e.EstudianteId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Estudiantes>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.Estudiantes.Where(criterio).AsNoTracking().ToListAsync();
    }

    private async Task<bool> Existe(int? id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == id);
    }
}

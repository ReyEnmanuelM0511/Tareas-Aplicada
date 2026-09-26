namespace RegistrodeLibros.Services;

using Aplicada1.Core;
using Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
using System.Linq.Expressions;

public class EstudiantesServices(IDbContextFactory<Contexto> contextFactory) : IService<Estudiante, int>
{

    public async Task<bool> Guardar(Estudiante estudiante)
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

    public async Task<bool> Insertar(Estudiante estudiante)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.estudiantes.Add(estudiante);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Estudiante estudiante)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        _context.Update(estudiante);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<Estudiante> Buscar(int id)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == id);
    }

    public async Task<bool> Eliminar(int id)
    {

        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.estudiantes.Where(e => e.EstudianteId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Estudiante>> GetList(Expression<Func<Estudiante, bool>> criterio)
    {
        await using var _context = await contextFactory.CreateDbContextAsync();
        return await _context.estudiantes.Where(criterio).AsNoTracking().ToListAsync();
    }

    private async Task<bool> Existe(int? id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.estudiantes
            .AnyAsync(e => e.EstudianteId == id);
    }
}

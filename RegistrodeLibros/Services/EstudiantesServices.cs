using Aplicada1.Core;
using RegistrodeLibros.Context;
using Microsoft.EntityFrameworkCore;
using RegistrodeLibros.Models;
using System.Linq.Expressions;

namespace RegistrodeLibros.Services;

public class EstudiantesServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Estudiantes, int>
{
    public async Task<Result> Guardar(Estudiantes estudiante)
    {
        if (!await Existe(estudiante.EstudianteId))
        {
            if (await ExisteNombre(estudiante.Nombres))
            {
                return Result.Failure(new Error("Nombre Duplicado","Ya existe un estudiante con ese nombre"));
            }

            var inserto = await Insertar(estudiante);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(estudiante);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int? estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.AnyAsync(e => e.EstudianteId == estudianteId);
    }

    private async Task<bool> ExisteNombre(string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.AnyAsync(e => e.Nombres == nombre);
    }

    private async Task<bool> Insertar(Estudiantes estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Estudiantes.Add(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Estudiantes estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Estudiantes?>> Buscar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);
    }

    public async Task<Result<List<Estudiantes>>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.Where(criterio).AsNoTracking().ToListAsync();
    }

    public async Task<Result> Eliminar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Estudiantes.Where(e => e.EstudianteId == estudianteId).ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }
}

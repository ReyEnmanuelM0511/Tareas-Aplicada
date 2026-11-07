using Microsoft.AspNetCore.Mvc;
using RegistroLibro.Service.Services;
using RegistroLibros.DATA.Models;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController : ControllerBase
{
    private readonly EstudiantesServices estudianteService;

    public EstudiantesController(EstudiantesServices estudianteService)
    {
        this.estudianteService = estudianteService;
    }

    // GET: api/estudiantes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estudiantes>>> GetEstudiantes()
    {
        var result = await estudianteService.GetList(e => true);
        return Ok(result.Value);
    }

    // GET: api/estudiantes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Estudiantes>> GetEstudiante(int id)
    {
        var result = await estudianteService.Buscar(id);

        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    // POST: api/estudiantes
    [HttpPost]
    public async Task<ActionResult<Estudiantes>> PostEstudiante(Estudiantes estudiante)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await estudianteService.Guardar(estudiante);
        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo guardar el estudiante.");
        }

        return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.EstudianteId }, estudiante);
    }

    // PUT: api/estudiantes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEstudiante(int id, Estudiantes estudiante)
    {
        if (id != estudiante.EstudianteId)
        {
            return BadRequest("El ID de la ruta no coincide con el ID del estudiante.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await estudianteService.Existe(id))
        {
            return NotFound();
        }

        var result = await estudianteService.Guardar(estudiante);
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el estudiante.");
        }

        return NoContent();
    }

    // DELETE: api/estudiantes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEstudiante(int id)
    {
        if (!await estudianteService.Existe(id))
        {
            return NotFound();
        }

        var result = await estudianteService.Eliminar(id);
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo eliminar el estudiante.");
        }

        return NoContent();
    }
}

using Microsoft.AspNetCore.Mvc;
using RegistroLibros.DATA.Models;
using RegistroLibro.Service.Services;

namespace RegistroLibros.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EstudianteController : ControllerBase
{
    private readonly EstudiantesServices estudianteServices;

    public EstudianteController(EstudiantesServices estudianteService)
    {
        estudianteServices = estudianteService;
    }

    //GET: api/estudiantes
    [HttpGet]
    public async Task <ActionResult<IEnumerable<Estudiantes>>> GetEstudaintes()
    {
        var result = await estudianteServices.GetList(e => true);
        return Ok(result.Value);
    }

    //GET: api/estudiantes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Estudiantes>> GetEstudiante(int id)
    {
        var result = await estudianteServices.Buscar(id);
        
        if(!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    //GET: api/estudiantes
    [HttpPost]
    public async Task<ActionResult<Estudiantes>> PostEstudiante(Estudiantes estudiante)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await estudianteServices.Guardar(estudiante);
        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo guardar el estudiante");
        }

        return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.EstudianteId }, estudiante);
    }

    //PUT: api/estudiantes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEstudiante(int id, Estudiantes estudiante)
    {
        if(id != estudiante.EstudianteId)
        {
            return BadRequest("El Id de la ruta no coincide con el Id del estudiante");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if(!await estudianteServices.Existe(id))
        {
            return NotFound();
        }

        var result = await estudianteServices.Guardar(estudiante);
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el estudiante");
        }

        return NoContent();
    }

    // DELETE: api/estudiantes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEstudiante(int id)
    {
        if (!await estudianteServices.Existe(id))
        {
            return NotFound();
        }

        var result = await estudianteServices.Eliminar(id);
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo eliminar el estudiante.");
        }

        return NoContent();
    }
}



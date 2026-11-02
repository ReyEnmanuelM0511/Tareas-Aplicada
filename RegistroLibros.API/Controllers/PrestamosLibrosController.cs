using Microsoft.AspNetCore.Mvc;
using RegistroLibro.Services;
using RegistroLibros.DATA.Models;
using RegistroLibro.Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController : ControllerBase
{
    private readonly PrestamosServices _prestamoService;

    public PrestamosController(PrestamosServices prestamoService)
    {
        _prestamoService = prestamoService;
    }

    // GET: api/prestamos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrestamoLibros>>> GetPrestamos()
    {
        var result = await _prestamoService.GetList(p => true);
        return Ok(result.Value);
    }

    // GET: api/prestamos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PrestamoLibros>> GetPrestamo(int id)
    {
        var result = await _prestamoService.Buscar(id);

        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    // POST: api/prestamos
    [HttpPost]
    public async Task<ActionResult<PrestamoLibros>> PostPrestamo(PrestamoLibros prestamo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _prestamoService.Guardar(prestamo);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo guardar el préstamo. Comprueba que el libro exista y esté disponible.");
        }

        return CreatedAtAction(nameof(GetPrestamo), new { id = prestamo.PrestamoId }, prestamo);
    }

    // PUT: api/prestamos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrestamo(int id, PrestamoLibros prestamo)
    {
        if (id != prestamo.PrestamoId)
        {
            return BadRequest("El ID de la ruta no coincide con el ID del préstamo.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _prestamoService.Existe(id))
        {
            return NotFound();
        }

        var result = await _prestamoService.Guardar(prestamo);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo actualizar el préstamo. Comprueba que el libro exista y esté disponible.");
        }

        return NoContent();
    }

    // DELETE: api/prestamos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrestamo(int id)
    {
        if (!await _prestamoService.Existe(id))
        {
            return NotFound();
        }

        var result = await _prestamoService.Eliminar(id);

        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo eliminar el préstamo.");
        }

        return NoContent();
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reto_Guardians.DBContext;
using reto_Guardians.DTO;
using reto_Guardians.Models;

namespace reto_Guardians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionesPController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelacionesPController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RelacionesPersonales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelacionPDTO>>> GetRelacionesP()
        {
            try
            {
                var rps = await _context.RelacionesPersonales.Select(a => new RelacionPDTO
                {

                    Id = a.IdRelacion,
                    Id1 = a.IdPersona1,
                    Nombre1 = a.IdPersona1Navigation.Nombre,
                    Id2 = a.IdPersona2,
                    Nombre2 = a.IdPersona2Navigation.Nombre,
                    TipoRelacion = a.TipoRelacion                   
                }).ToListAsync();

                if (rps == null || rps.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(rps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/RelacionesPersonales/BuscarRelacionId/1
        [HttpGet("BuscarRelacionId/{idrp}")]
        public async Task<ActionResult<RelacionPDTO>> GetRelacionP(int idrp)
        {
            try
            {
                var rp = await _context.RelacionesPersonales
                .FirstOrDefaultAsync(p => p.IdPersona1 == idrp || p.IdPersona2==idrp);
                if (rp == null)
                {
                    return NotFound($"No existe la relación con id {idrp}");
                }
                var relacion = new RelacionPDTO {
                    Id = rp.IdRelacion,
                    Id1 = rp.IdPersona1,
                    Nombre1 = rp.IdPersona1Navigation.Nombre,
                    Id2 = rp.IdPersona2,
                    Nombre2 = rp.IdPersona2Navigation.Nombre,
                    TipoRelacion = rp.TipoRelacion
                };
                return Ok(relacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/RelacionesPersonales/ModificarRelacion/5
        [HttpPut("ModificarRelacion/{idrp}")]
        public async Task<IActionResult> PutRelacionP(int idrp, [FromBody] RelacionPDTO rpUpdate)
        {
            var existingRp = await _context.RelacionesPersonales.FindAsync(idrp);
            if (existingRp == null)
            {
                return NotFound($"No se encontró la realación personal con el id {idrp}");
            }
            if (!string.IsNullOrEmpty(rpUpdate.TipoRelacion))
            {
                existingRp.TipoRelacion = rpUpdate.TipoRelacion;
            }
            if (rpUpdate.Id1 != null && rpUpdate.Id1 > 0)
            {
                existingRp.IdPersona1 = rpUpdate.Id1.Value;
            }
            if (rpUpdate.Id2 != null && rpUpdate.Id2 > 0)
            {
                existingRp.IdPersona2 = rpUpdate.Id2.Value;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelacionpExists(idrp))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // POST: api/RelacionesPersonales/CrearRelacion
        [HttpPost("CrearRelacion")]
        public async Task<ActionResult<RelacionPersonal>> PostPersona([FromBody] RelacionPDTO nueva)
        {
            try
            {
                var relacionNueva = new RelacionPersonal
                {
                    TipoRelacion = nueva.TipoRelacion ?? string.Empty,
                    IdPersona1 = nueva.Id1.HasValue ? nueva.Id1.Value : 0,
                    IdPersona2 = nueva.Id2.HasValue ? nueva.Id2.Value : 0
                };
                _context.RelacionesPersonales.Add(relacionNueva);
                await _context.SaveChangesAsync();
                return Ok(relacionNueva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/RelacionesPersonales/BorrarRelacion/5
        [HttpDelete("BorrarRelacion/{idpersona}")]
        public async Task<IActionResult> DeleteRelacion(int idrp)
        {
            if (_context.RelacionesPersonales == null)
            {
                return NotFound();
            }
            var relacion = await _context.RelacionesPersonales.Where(a => a.IdRelacion == idrp).FirstAsync();
            if (relacion == null)
            {
                return NotFound();
            }
            _context.RelacionesPersonales.Remove(relacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RelacionpExists(int id)
        {
            return (_context.RelacionesPersonales?.Any(e => e.IdRelacion == id)).GetValueOrDefault();
        }
    }
}

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
    public class PersonasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonaDTO>>> GetPersonas()
        {
            try
            {
                var personas = await _context.Personas.Select(a => new PersonaDTO
                {
               
                    Id = a.IdPersona,
                    Nombre = a.Nombre,
                    Edad = a.Edad
                }).ToListAsync();

                if (personas == null || personas.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(personas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Personas/BuscarPersonasId/1
        [HttpGet("BuscarPersonasId/{idpersona}")]
        public async Task<ActionResult<Persona>> GetPersona(int idpersona)
        {
            try
            {
                var persona = await _context.Personas
                .FirstOrDefaultAsync(p => p.IdPersona == idpersona);
                if (persona == null)
                {
                    return NotFound($"No existe la persona con id {idpersona}");
                }
                return Ok(persona);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Personas/ModificarPersona/5
        [HttpPut("ModificarPersonas/{idpersona}")]
        public async Task<IActionResult> PutPersona(int idpersona, [FromBody] PersonaDTO personaUpdate)
        {
            var existingPersona = await _context.Personas.FindAsync(idpersona);
            if (existingPersona == null)
            {
                return NotFound($"No se encontró la persona con el id {idpersona}");
            }
            if (!string.IsNullOrEmpty(personaUpdate.Nombre))
            {
                existingPersona.Nombre = personaUpdate.Nombre;
            }
            if (personaUpdate.Edad != null && personaUpdate.Edad > 0)
            {
                existingPersona.Edad = personaUpdate.Edad.Value;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(idpersona))
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
        // POST: api/Personas/CrearPersona
        [HttpPost("CrearPersona")]
        public async Task<ActionResult<Persona>> PostPersona([FromBody] PersonaDTO nueva)
        {
            try
            {
                    var personaNueva = new Persona
                    {
                        Nombre = nueva.Nombre ?? string.Empty,
                        Edad = nueva.Edad.HasValue ? nueva.Edad.Value : 0
                    };
                    _context.Personas.Add(personaNueva);
                    await _context.SaveChangesAsync();
                    return Ok(personaNueva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/Personas/BorrarPersona/5
        [HttpDelete("BorrarPersona/{idpersona}")]
        public async Task<IActionResult> DeleteHeroe(int idpersona)
        {
            if (_context.Personas == null)
            {
                return NotFound();
            }
            var persona = await _context.Personas.Where(a => a.IdPersona == idpersona).FirstAsync();
            if (persona == null)
            {
                return NotFound();
            }
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PersonaExists(int id)
        {
            return (_context.Personas?.Any(e => e.IdPersona == id)).GetValueOrDefault();
        }
    }
}

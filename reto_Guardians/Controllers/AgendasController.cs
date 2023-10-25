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
    public class AgendasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgendasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Agendas
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgendaDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetAgendas()
        {
            try
            {
                var evento = await _context.Agenda.Select(a=> new AgendaDTO
                {
                    IdEvento = a.IdEvento,
                    Evento = a.Evento,
                    IdHeroe = a.IdHeroe,
                    Heroe = a.IdHeroeNavigation.Alias,
                    Descripcion = a.Descripcion,
                    Fecha = a.Fecha
                }).ToListAsync();
                
                if (evento == null || evento.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Agendas/BuscarAgendaId/1
        [HttpGet("BuscarAgendaId/{idevento}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgendaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AgendaDTO>> GetAgenda(int idevento)
        {
            try
            {
                if(idevento <= 0)
                {
                    return BadRequest("El id de evento no puede ser 0 o menor a este");
                }
                var agendum = await _context.Agenda
                .Include(h => h.IdHeroeNavigation)
                .FirstOrDefaultAsync(h => h.IdEvento == idevento);
                if (agendum == null)
                {
                    return NotFound($"No existe el evento con id {idevento}");
                }
                var evento = new AgendaDTO
                {
                    IdEvento = agendum.IdEvento,
                    Evento = agendum.Evento,
                    IdHeroe = agendum.IdHeroe,
                    Heroe = agendum.IdHeroeNavigation.Alias,
                    Descripcion = agendum.Descripcion,
                    Fecha = agendum.Fecha
                };
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Agendas/BuscarAgendaAlias/Batman
        [HttpGet("BuscarAgendaAlias/{alias}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgendaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetAgendaAlias(string alias)
        {
            try {
                if(string.IsNullOrEmpty(alias))
                {
                    return BadRequest("Debe proporcionarse un alias para la búsqueda");
                }
                var heroe = await _context.Heroes.SingleOrDefaultAsync(h => h.Alias == alias);

                if (heroe == null)
                {
                    return NotFound($"No existe el héroe {alias}.");
                }

                if (_context.Agenda == null)
                {
                    return NotFound("La entidad 'Agenda' es nula.");
                }

                var agendaData = await _context.Agenda
                    .Where(a => a.IdHeroeNavigation.Alias == alias)
                    .Select(a => new AgendaDTO
                    {
                        IdEvento = a.IdEvento,
                        Evento = a.Evento,
                        IdHeroe = a.IdHeroe,
                        Heroe = a.IdHeroeNavigation.Alias,
                        Descripcion = a.Descripcion,
                        Fecha = a.Fecha
                    })
                    .ToListAsync();

                if (!agendaData.Any())
                {
                    return NotFound($"No existen eventos para {alias}");
                }

                return Ok(agendaData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            
        }

        // GET: api/Agendas/BuscarAgendaFecha/2023-10-11
        [HttpGet("BuscarAgendaFecha/{fecha}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgendaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AgendaDTO>>> GetAgendaPorFecha(DateTime fecha)
        {
            try {
                if (fecha == DateTime.MinValue)
                {
                    return BadRequest("La fecha ingresada no es válida.");
                }
                if (_context.Agenda == null)
                {
                    return NotFound("La entidad 'Agenda' es nula.");
                }
                var eventosEnFecha = await _context.Agenda.Where(agenda => agenda.Fecha == fecha).Select(agenda => new AgendaDTO
                {
                    IdEvento = agenda.IdEvento,
                    IdHeroe = agenda.IdHeroe,
                    Heroe = agenda.IdHeroeNavigation.Alias,
                    Evento = agenda.Evento,
                    Descripcion = agenda.Descripcion,
                    Fecha = agenda.Fecha
                }).ToListAsync();
                if (eventosEnFecha.Count == 0)
                {
                    return NotFound($"No existen eventos para {fecha}");
                }

                return Ok(eventosEnFecha);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            
        }

        // PUT: api/Agendas/ModificarAgenda/5
        [HttpPut("ModificarAgenda/{idevento}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAgenda(int idevento, [FromBody] AgendaDTO agendaUpdate)
        {
            if (idevento <= 0)
            {
                return BadRequest("El id de evento no puede ser 0 o menor a este");
            }
            var existingAgenda = await _context.Agenda.FindAsync(idevento);
            if (existingAgenda == null)
            {
                return NotFound($"No se encontró el evento con el id {idevento}");
            }
            if (!string.IsNullOrEmpty(agendaUpdate.Evento))
            {
                existingAgenda.Evento = agendaUpdate.Evento;
            }
            if (!string.IsNullOrEmpty(agendaUpdate.Descripcion))
            {
                existingAgenda.Descripcion = agendaUpdate.Descripcion;
            }
            if (agendaUpdate.Fecha != null && agendaUpdate.Fecha != DateTime.MinValue)
            {
                existingAgenda.Fecha = agendaUpdate.Fecha.Value;
            }

            if (agendaUpdate.IdHeroe != null && agendaUpdate.IdHeroe != 0)
            {
                existingAgenda.IdHeroe = agendaUpdate.IdHeroe.Value;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgendaExists(idevento))
                {
                    return NotFound($"No existe el evento con id {idevento}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Agendas/CrearAgenda
        [HttpPost("CrearAgenda")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgendaDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Agendum>> PostAgenda([FromBody] AgendaDTO nuevo)
        {

            if (_context.Agenda == null)
            {
                return Problem("Entity set 'AppDbContext.Agenda' is null.");
            }
            var eventoExistente = await _context.Agenda
                .FirstOrDefaultAsync(a => a.Descripcion == nuevo.Descripcion && a.Fecha == nuevo.Fecha);

            if (eventoExistente != null)
            {
                return Conflict("Ya existe un evento con la misma descripción y fecha.");
            }
            var agendaNueva = new Agendum
            {
                IdHeroe = nuevo.IdHeroe ?? 0,
                Descripcion = nuevo.Descripcion ?? "",
                Fecha = nuevo.Fecha ?? DateTime.Now,
                Evento = nuevo.Evento ?? "Desconocido"
            };
            _context.Agenda.Add(agendaNueva);
            await _context.SaveChangesAsync();
            return Ok(agendaNueva);

        }

        // DELETE: api/Agendas/BorrarAgenda/5
        [HttpDelete("BorrarAgenda/{idevento}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAgenda(int idevento)
        {
            try {
                if (idevento <= 0)
                {
                    return BadRequest("El id de evento no puede ser 0 o menor a este");
                }
                if (_context.Agenda == null)
                {
                    return NotFound("No hay registros");
                }
                var agendum = await _context.Agenda.Where(a => a.IdEvento == idevento).FirstAsync();
                if (agendum == null)
                {
                    return NotFound($"No existe el evento con id {idevento}");
                }
                _context.Agenda.Remove(agendum);
                await _context.SaveChangesAsync();
                return NoContent();
            } catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            
        }

        private bool AgendaExists(int id)
        {
            return (_context.Agenda?.Any(e => e.IdEvento == id)).GetValueOrDefault();
        }
    }
}

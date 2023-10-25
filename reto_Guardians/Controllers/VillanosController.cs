using MessagePack;
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
    public class VillanosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VillanosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Villanos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Villano))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VillanoDTO>>> GetVillanos()
        {
            try
            {
                var villanos = await _context.Villanos.Select(a => new VillanoDTO
                {
                    IdVillano = a.IdVillano,
                    Alias = a.Alias,
                    IdPersona = a.IdPersona,
                    Nombre = a.IdPersonaNavigation.Nombre,
                    Edad = a.IdPersonaNavigation.Edad,
                    Origen = a.Origen,
                    Poder = a.Poder,
                    Debilidad = a.Debilidad
                }).ToListAsync();

                if (villanos == null || villanos.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(villanos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Villanos/BuscarVillanoId/1
        [HttpGet("BuscarVillanoId/{idvillano}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillanoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillanoDTO>> GetVillano(int idvillano)
        {
            try
            {
                if (idvillano <= 0)
                {
                    return BadRequest("El id del villano no puede ser 0 o menor a este");
                }
                var villano = await _context.Villanos
                .Include(v => v.IdPersonaNavigation)
                .FirstOrDefaultAsync(v => v.IdVillano == idvillano);
                if (villano == null)
                {
                    return NotFound($"No existe el heroe con id {idvillano}");
                }
                var vil = new VillanoDTO
                {
                    IdVillano = villano.IdVillano,
                    Alias = villano.Alias,
                    IdPersona = villano.IdPersona,
                    Nombre = villano.IdPersonaNavigation.Nombre,
                    Edad = villano.IdPersonaNavigation.Edad,
                    Origen = villano.Origen,
                    Poder = villano.Poder,
                    Debilidad = villano.Debilidad
                };
                return Ok(vil);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        // GET: api/Villanos/BuscarVillanoAlias/Joker
        [HttpGet("BuscarVillanoAlias/{villano}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillanoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillanoDTO>> GetVillanoAlias(string villano)
        {
            try
            {
                if (string.IsNullOrEmpty(villano))
                {
                    return BadRequest("Debe proporcionarse un alias para la búsqueda");
                }
                var vil = await _context.Villanos
                .Include(v => v.IdPersonaNavigation)
                .FirstOrDefaultAsync(v => v.Alias == villano);
                if (vil == null)
                {
                    return NotFound($"No existe {villano}");
                }
                var v = new VillanoDTO
                {
                    IdVillano = vil.IdVillano,
                    Alias = vil.Alias,
                    IdPersona = vil.IdPersona,
                    Nombre = vil.IdPersonaNavigation.Nombre,
                    Edad = vil.IdPersonaNavigation.Edad,
                    Origen = vil.Origen,
                    Poder = vil.Poder,
                    Debilidad = vil.Debilidad
                };
                return Ok(v);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Villanos/BuscarVillanoOrigen/Explosión Laboratorio
        [HttpGet("BuscarVillanoOrigen/{origen}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillanoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VillanoDTO>>> GetVillanosOrigen(string origen)
        {
            try
            {
                if (_context.Villanos == null)
                {
                    return NotFound("La entidad 'Villanos' es nula.");
                }
                if (string.IsNullOrEmpty(origen))
                {
                    return BadRequest("Debe proporcionarse un origen para la búsqueda");
                }
                var villanosConOrigen = await _context.Villanos.Where(v => v.Origen == origen).Select(v => new VillanoDTO
                {
                    IdVillano = v.IdVillano,
                    Alias = v.Alias,
                    IdPersona = v.IdPersona,
                    Nombre = v.IdPersonaNavigation.Nombre,
                    Edad = v.IdPersonaNavigation.Edad,
                    Origen = v.Origen,
                    Poder = v.Poder,
                    Debilidad = v.Debilidad
                }).ToListAsync();
                if (villanosConOrigen == null || villanosConOrigen.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(villanosConOrigen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Villanos/BuscarVillanosDebilidad/Fuego
        [HttpGet("BuscarVillanosDebilidad/{debilidad}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillanoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VillanoDTO>>> GetVillanosDebilidad(string debilidad)
        {
            try
            {
                if (_context.Villanos == null)
                {
                    return NotFound("La entidad 'Villanos' es nula.");
                }
                if (string.IsNullOrEmpty(debilidad))
                {
                    return BadRequest("Debe proporcionarse una debilidad para la búsqueda");
                }
                var villanosConDebilidad = await _context.Villanos.Where(v => v.Debilidad == debilidad).Select(v => new VillanoDTO
                {
                    IdVillano = v.IdVillano,
                    Alias = v.Alias,
                    IdPersona = v.IdPersona,
                    Nombre = v.IdPersonaNavigation.Nombre,
                    Edad = v.IdPersonaNavigation.Edad,
                    Origen = v.Origen,
                    Poder = v.Poder,
                    Debilidad = v.Debilidad
                }).ToListAsync();
                if (villanosConDebilidad == null || villanosConDebilidad.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(villanosConDebilidad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Villanos/MasDerrotado

        [HttpGet("MasDerrotado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillanoDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillanoDTO>> MasDerrotado()
        {
            try
            {
                var villanoMasPerdedor = await _context.Villanos
                .Include(v => v.IdPersonaNavigation)
                .Where(v => _context.Combates
                .Where(c => c.Resultado == "Victoria heroe")
                .Where(c => c.IdHeroeNavigation.IdPersonaNavigation.Edad < 18)
                .Any(c => c.IdVillano == v.IdVillano))
                .OrderByDescending(v => _context.Combates
                    .Where(c => c.Resultado == "Victoria heroe")
                    .Where(c => c.IdHeroeNavigation.IdPersonaNavigation.Edad < 18)
                    .Count(c => c.IdVillano == v.IdVillano))
                .Take(1)
                .SingleOrDefaultAsync();

                if (villanoMasPerdedor == null)
                {
                    return NotFound("No se encontró ningún villano que haya perdido contra héroes adolescentes.");
                }

                var villanoDTO = new VillanoDTO
                {
                    IdVillano = villanoMasPerdedor.IdVillano,
                    Alias = villanoMasPerdedor.Alias,
                    IdPersona = villanoMasPerdedor.IdPersona,
                    Nombre = villanoMasPerdedor.IdPersonaNavigation.Nombre,
                    Edad = villanoMasPerdedor.IdPersonaNavigation.Edad,
                    Origen = villanoMasPerdedor.Origen,
                    Poder = villanoMasPerdedor.Poder,
                    Debilidad = villanoMasPerdedor.Debilidad
                };

                return Ok(villanoDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Villano/ModificarVillano/5
        [HttpPut("ModificarVillano/{idvillano}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutVillano(int idvillano, [FromBody] VillanoDTO VillanoUpdate)
        {
            if (idvillano <= 0)
            {
                return BadRequest("El id del villano no puede ser 0 o menor a este");
            }
            var existingVillano = await _context.Villanos.FindAsync(idvillano);
            if (existingVillano == null)
            {
                return NotFound($"No se encontró el heroe con el id {idvillano}");
            }
            if (!string.IsNullOrEmpty(VillanoUpdate.Alias))
            {
                existingVillano.Alias = VillanoUpdate.Alias;
            }
            if (!string.IsNullOrEmpty(VillanoUpdate.Poder))
            {
                existingVillano.Poder = VillanoUpdate.Poder;
            }
            if (!string.IsNullOrEmpty(VillanoUpdate.Debilidad))
            {
                existingVillano.Debilidad = VillanoUpdate.Debilidad;
            }
            if (!string.IsNullOrEmpty(VillanoUpdate.Origen))
            {
                existingVillano.Origen = VillanoUpdate.Origen;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VillanoExists(idvillano))
                {
                    return NotFound($"No existe el villano con id {idvillano}");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // POST: api/Villanos/CrearVillano
        [HttpPost("CrearVillano")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillanoDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Villano>> PostVillano([FromBody] VillanoDTO nuevo)
        {
            try
            {
                var villanoExistente = await _context.Villanos
                    .FirstOrDefaultAsync(a => a.Alias == nuevo.Alias);
                if (villanoExistente != null)
                {
                    return Conflict($"El villano {nuevo.Alias} ya existe");
                }
                if (nuevo.IdPersona != null)
                {   
                    var villanoNuevo = new Villano
                    {
                        Alias = nuevo.Alias ?? string.Empty,
                        IdPersona = nuevo.IdPersona.Value,
                        Poder = nuevo.Poder ?? "Ninguno",
                        Debilidad = nuevo.Debilidad ?? "Ninguna",
                        Origen = nuevo.Origen ?? "Desconocido"
                    };

                    _context.Villanos.Add(villanoNuevo);
                    await _context.SaveChangesAsync();
                    return Ok(villanoNuevo);
                }
                else
                {
                    var personaExistente = await _context.Personas
                    .FirstOrDefaultAsync(a => a.Nombre == nuevo.Nombre && a.Edad == nuevo.Edad);
                    if (personaExistente != null)
                    {
                        return Conflict($"La persona {nuevo.Nombre} ya existe");
                    }
                    var persona = new Persona
                    {
                        Nombre = nuevo.Nombre ?? string.Empty,
                        Edad = nuevo.Edad ?? 1
                    };

                    _context.Personas.Add(persona);
                    await _context.SaveChangesAsync();
                    var villanoNuevo = new Villano
                    {
                        Alias = nuevo.Alias ?? string.Empty,
                        IdPersona = persona.IdPersona,
                        Poder = nuevo.Poder ?? "Ninguno",
                        Debilidad = nuevo.Debilidad ?? "Ninguna",
                        Origen = nuevo.Origen ?? "Desconocido"
                    };

                    _context.Villanos.Add(villanoNuevo);
                    await _context.SaveChangesAsync();
                    return Ok(villanoNuevo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/Villano/BorrarVillano/5
        [HttpDelete("BorrarVillano/{idheroe}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVillano(int idvillano)
        {
            if (_context.Villanos == null)
            {
                return NotFound();
            }
            if (idvillano<= 0)
            {
                return BadRequest("El id del villano no puede ser 0 o menor a este");
            }
            var villano = await _context.Villanos.Where(a => a.IdVillano == idvillano).FirstAsync();
            if (villano == null)
            {
                return NotFound();
            }
            _context.Villanos.Remove(villano);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool VillanoExists(int id)
        {
            return (_context.Villanos?.Any(e => e.IdVillano == id)).GetValueOrDefault();
        }
    }
}

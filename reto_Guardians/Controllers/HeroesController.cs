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
    public class HeroesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HeroesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Heroes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HeroeDTO>>> GetHeroes()
        {
            try
            {
                var heroes = await _context.Heroes.Select(a => new HeroeDTO
                {
                    IdHeroe = a.IdHeroe,
                    Alias = a.Alias,
                    IdPersona = a.IdPersona,
                    Nombre = a.IdPersonaNavigation.Nombre,
                    Edad = a.IdPersonaNavigation.Edad,
                    Poder= a.Poder,
                    Debilidad = a.Debilidad
                }).ToListAsync();

                if (heroes == null || heroes.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(heroes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Heroes/BuscarHeroeId/1
        [HttpGet("BuscarHeroeId/{idheroe}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HeroeDTO>> GetHeroe(int idheroe)
        {
            try
            {
                if (idheroe <= 0)
                {
                    return BadRequest("El id del heroe no puede ser 0 o menor a este");
                }
                var heroe = await _context.Heroes
                .Include(h => h.IdPersonaNavigation)
                .FirstOrDefaultAsync(h => h.IdHeroe == idheroe);
                if (heroe == null)
                {
                    return NotFound($"No existe el heroe con id {idheroe}");
                }
                var h = new HeroeDTO
                {
                    IdHeroe = heroe.IdHeroe,
                    Alias = heroe.Alias,
                    IdPersona = heroe.IdPersona,
                    Nombre = heroe.IdPersonaNavigation?.Nombre,
                    Edad = heroe.IdPersonaNavigation?.Edad,
                    Poder = heroe.Poder,
                    Debilidad = heroe.Debilidad
                };
                return Ok(h);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        // GET: api/Heroes/BuscarHeroeAlias/Batman
        [HttpGet("BuscarHeroeAlias/{heroe}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<HeroeDTO>> GetHeroeAlias(string heroe)
        {
            try
            {
                if (string.IsNullOrEmpty(heroe))
                {
                    return BadRequest("Debe proporcionarse un alias para la búsqueda");
                }
                var hero = await _context.Heroes
                .Include(h => h.IdPersonaNavigation)
                .FirstOrDefaultAsync(h => h.Alias == heroe);
                if (hero == null)
                {
                    return NotFound($"No existe {heroe}");
                }
                var h = new HeroeDTO
                {
                    IdHeroe = hero.IdHeroe,
                    Alias = hero.Alias,
                    IdPersona = hero.IdPersona,
                    Nombre = hero.IdPersonaNavigation.Nombre,
                    Edad = hero.IdPersonaNavigation.Edad,
                    Poder = hero.Poder,
                    Debilidad = hero.Debilidad
                };
                return Ok(h);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Heroes/BuscarHeroePoder/Volar
        [HttpGet("BuscarHeroePoder/{poder}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HeroeDTO>>> GetHeroesPoder(string poder)
        {
            try
            {
                if (_context.Heroes == null)
                {
                    return NotFound("La entidad 'Heroes' es nula.");
                }
                if (string.IsNullOrEmpty(poder))
                {
                    return BadRequest("Debe proporcionarse un poder para la búsqueda");
                }
                var heroesConPoder = await _context.Heroes.Where(h => h.Poder == poder).Select(h => new HeroeDTO
                {
                    IdHeroe = h.IdHeroe,
                    Alias = h.Alias,
                    IdPersona = h.IdPersona,
                    Nombre = h.IdPersonaNavigation.Nombre,
                    Edad = h.IdPersonaNavigation.Edad,
                    Poder = h.Poder,
                    Debilidad = h.Debilidad
                }).ToListAsync();
                if (heroesConPoder == null || heroesConPoder.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(heroesConPoder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Heroes/BuscarHeroePoder/Familiar
        [HttpGet("BuscarHeroeRelacion/{relacion}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HeroeDTO>>> GetHeroesRelacion(string relacion)
        {
            try
            {
                if (_context.Heroes == null)
                {
                    return NotFound("La entidad 'Heroes' es nula.");
                }
                if (string.IsNullOrEmpty(relacion))
                {
                    return BadRequest("Debe proporcionarse una relación para la búsqueda");
                }
                var heroesConRelacion = await _context.Heroes
                .Where(h => _context.RelacionesPersonales
                    .Any(rp => rp.IdPersona1 == h.IdPersona && rp.TipoRelacion == relacion || rp.IdPersona2 == h.IdPersona && rp.TipoRelacion == relacion))
                .Select(heroe => new HeroeDTO
                    {
                    IdHeroe = heroe.IdHeroe,
                    Alias = heroe.Alias,
                    IdPersona = heroe.IdPersona,
                    Nombre = heroe.IdPersonaNavigation.Nombre,
                    Edad = heroe.IdPersonaNavigation.Edad,
                    Poder = heroe.Poder,
                    Debilidad = heroe.Debilidad
                    }).ToListAsync();
                if (heroesConRelacion == null || heroesConRelacion.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(heroesConRelacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Heroes/PorEdad
        [HttpGet("PorEdad")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HeroeDTO>>> GetHeroesEdad()
        {
            try
            {
                var heroes = await _context.Heroes.Select(a => new HeroeDTO
                {
                    IdHeroe = a.IdHeroe,
                    Alias = a.Alias,
                    IdPersona = a.IdPersona,
                    Nombre = a.IdPersonaNavigation.Nombre,
                    Edad = a.IdPersonaNavigation.Edad,
                    Poder = a.Poder,
                    Debilidad = a.Debilidad
                }).ToListAsync();

                if (heroes == null || heroes.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                var heroesAgrupadosPorEdad = heroes
                .GroupBy(h => h.Edad < 18 ? "Adolescentes" : "Mayores de edad")
                .ToDictionary(group => group.Key, group => group.ToList());
                return Ok(heroesAgrupadosPorEdad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Heroes/Top3
        [HttpGet("Top3")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<HeroeDTO>>> Top3()
        {
            try
            {
                var heroes = await _context.Heroes.Select(a => new HeroeDTO
                {
                    IdHeroe = a.IdHeroe,
                    Alias = a.Alias,
                    IdPersona = a.IdPersona,
                    Nombre = a.IdPersonaNavigation.Nombre,
                    Edad = a.IdPersonaNavigation.Edad,
                    Poder = a.Poder,
                    Debilidad = a.Debilidad
                }).ToListAsync();

                if (heroes == null || heroes.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                var heroesConVictorias = heroes
                .Select(h => new
                {
                    Heroe = h,
                    Victorias = _context.Combates
                        .Count(c => c.IdHeroe == h.IdHeroe && c.Resultado == "Victoria heroe")
                })
                .OrderByDescending(h => h.Victorias)
                .Take(3)
                .Select(h => h.Heroe)
                .ToList();
                return Ok(heroesConVictorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Heroes/ModificarHeroe/5
        [HttpPut("ModificarHeroe/{idheroe}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutHeroe(int idheroe, [FromBody] HeroeDTO HeroeUpdate)
        {
            if (idheroe <= 0)
            {
                return BadRequest("El id del heroe no puede ser 0 o menor a este");
            }
            var existingHeroe = await _context.Heroes.FindAsync(idheroe);
            if (existingHeroe == null)
            {
                return NotFound($"No se encontró el heroe con el id {idheroe}");
            }
            if (!string.IsNullOrEmpty(HeroeUpdate.Alias))
            {
                existingHeroe.Alias = HeroeUpdate.Alias;
            }
            if (!string.IsNullOrEmpty(HeroeUpdate.Poder))
            {
                existingHeroe.Poder = HeroeUpdate.Poder;
            }
            if (!string.IsNullOrEmpty(HeroeUpdate.Debilidad))
            {
                existingHeroe.Debilidad = HeroeUpdate.Debilidad;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroeExists(idheroe))
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
        // POST: api/Heroes/CrearHeroe
        [HttpPost("CrearHeroe")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HeroeDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Heroe>> PostHeroe([FromBody] HeroeDTO nuevo)
        {
                var heroeExistente = await _context.Heroes
                .FirstOrDefaultAsync(a => a.Alias == nuevo.Alias);
                if (heroeExistente != null)
                {
                    return Conflict($"El heroe {nuevo.Alias} ya existe");
                }
                if (nuevo.IdPersona != null)
                {
                    var heroeNuevo = new Heroe
                    {
                        Alias = nuevo.Alias ?? string.Empty,
                        IdPersona = nuevo.IdPersona.Value,
                        Poder = nuevo.Poder ?? "Ninguno",
                        Debilidad = nuevo.Debilidad ?? "Ninguna"
                    };

                    _context.Heroes.Add(heroeNuevo);
                    await _context.SaveChangesAsync();
                    return Ok(heroeNuevo);
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
                    var heroeNuevo = new Heroe
                    {
                        Alias = nuevo.Alias ?? string.Empty,
                        IdPersona = persona.IdPersona,
                        Poder = nuevo.Poder ?? "Ninguno",
                        Debilidad = nuevo.Debilidad ?? "Ninguna"
                    };
                    _context.Heroes.Add(heroeNuevo);
                    await _context.SaveChangesAsync();
                    return Ok(heroeNuevo);
                }
        }

        // DELETE: api/Heroes/BorrarHeroe/5
        [HttpDelete("BorrarHeroe/{idheroe}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHeroe(int idheroe)
        {
            if (_context.Heroes == null)
            {
                return NotFound("No existen registros");
            }
            if (idheroe <= 0)
            {
                return BadRequest("El id del heroe no puede ser 0 o menor a este");
            }
            var heroe = await _context.Heroes.Where(a => a.IdHeroe == idheroe).FirstAsync();
            if (heroe == null)
            {
                return NotFound($"El heroe con id {idheroe} no existe");
            }
            _context.Heroes.Remove(heroe);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool HeroeExists(int id)
        {
            return (_context.Heroes?.Any(e => e.IdHeroe == id)).GetValueOrDefault();
        }
    }
}

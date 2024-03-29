﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reto_Guardians.DBContext;
using reto_Guardians.DTO;
using reto_Guardians.Models;

namespace reto_Guardians.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatrocinadoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatrocinadoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Patrocinadores
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatrocinadorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PatrocinadorDTO>>> GetPatrocinadores()
        {
            try
            {
                var patrocinadores = await _context.Patrocinadores.Select(a => new PatrocinadorDTO
                {
                    IdPatrocinador = a.IdPatrocinador,
                    Patrocinador = a.Nombre,
                    IdHeroe = a.IdHeroe,
                    Heroe = a.IdHeroeNavigation.Alias,
                    Monto = a.Monto,
                    OrigenDinero = a.Origen
                }).ToListAsync();

                if (patrocinadores == null || patrocinadores.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(patrocinadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        // GET: api/Patrocinadores/BuscarPatrocinadorId/1
        [HttpGet("BuscarPatrocinadorId/{idpatrocinador}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatrocinadorDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatrocinadorDTO>> GetPatrocinador(int idpatrocinador)
        {
            try
            {
                if (idpatrocinador <= 0)
                {
                    return BadRequest("El id del patrocinador no puede ser 0 o menor a este");
                }
                var patrocinador = await _context.Patrocinadores
                .Include(h => h.IdHeroeNavigation)
                .FirstOrDefaultAsync(h => h.IdPatrocinador == idpatrocinador);
                if (patrocinador == null)
                {
                    return NotFound($"No existe el Patrocinador con id {idpatrocinador}");
                }
                var pat = new PatrocinadorDTO
                {
                    IdPatrocinador = patrocinador.IdPatrocinador,
                    Patrocinador = patrocinador.Nombre,
                    IdHeroe = patrocinador.IdHeroe,
                    Heroe = patrocinador.IdHeroeNavigation.Alias,
                    Monto = patrocinador.Monto,
                    OrigenDinero = patrocinador.Origen
                };
                return Ok(pat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Patrocinadores/BuscarMayorPatrocinador/Batman
        [HttpGet("BuscarMayorPatrocinador/{alias}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatrocinadorDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PatrocinadorDTO>>> GetMayorPatrocinador(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                return BadRequest("Se debe proporcionar un alias para la búsqueda");
            }
            var heroe = await _context.Heroes
            .Include(h => h.Patrocinadors)
            .SingleOrDefaultAsync(h => h.Alias == alias);

            if (heroe == null)
            {
                return NotFound($"No existe el héroe con el alias {alias}.");
            }

            if (heroe.Patrocinadors.Count == 0)
            {
                return NotFound($"{alias} no tiene patrocinadores.");
            }

            var patrocinadorMasGeneroso = heroe.Patrocinadors
            .OrderByDescending(p => p.Monto)
            .FirstOrDefault();

            if (patrocinadorMasGeneroso == null)
            {
                return NotFound($"{alias} no tiene patrocinadores con montos válidos.");
            }

            var patrocinador = new PatrocinadorDTO
            {
                IdPatrocinador = patrocinadorMasGeneroso.IdPatrocinador,
                Patrocinador = patrocinadorMasGeneroso.Nombre,
                IdHeroe = patrocinadorMasGeneroso.IdHeroe,
                Heroe = patrocinadorMasGeneroso.IdHeroeNavigation.Alias,
                Monto = patrocinadorMasGeneroso.Monto,
                OrigenDinero = patrocinadorMasGeneroso.Origen
            };

            return Ok(patrocinador);
        }

        // PUT: api/Patrocinadores/ModificarPatrocinador/5
        [HttpPut("ModificarPatrocinador/{idpatrocinador}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutPatrocinador(int idpatrocinador, [FromBody] PatrocinadorDTO patrocinadorUpdate)
        {
            if (idpatrocinador <= 0)
            {
                return BadRequest("El id del patrocinador no puede ser 0 o menor a este");
            }
            var existingPatrocinador = await _context.Patrocinadores.FindAsync(idpatrocinador);
            if (existingPatrocinador == null)
            {
                return NotFound($"No se encontró el evento con el id {idpatrocinador}");
            }
            if (!string.IsNullOrEmpty(patrocinadorUpdate.Patrocinador))
            {
                existingPatrocinador.Nombre = patrocinadorUpdate.Patrocinador;
            }
            if (!string.IsNullOrEmpty(patrocinadorUpdate.OrigenDinero))
            {
                existingPatrocinador.Origen = patrocinadorUpdate.OrigenDinero;
            }

            if (patrocinadorUpdate.IdHeroe != null && patrocinadorUpdate.IdHeroe != 0)
            {
                existingPatrocinador.IdHeroe = patrocinadorUpdate.IdHeroe.Value;
            }
            if (patrocinadorUpdate.Monto != null && patrocinadorUpdate.Monto >= 0)
            {
                existingPatrocinador.Monto = patrocinadorUpdate.Monto.Value;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatrocinadorExists(idpatrocinador))
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

        // POST: api/Patrocinadores/CrearPatrocinador
        [HttpPost("CrearPatrocinador")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatrocinadorDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Patrocinador>> PostPatrocinador([FromBody] PatrocinadorDTO nuevo)
        {
            if (_context.Patrocinadores == null)
            {
                return Problem("Entity set 'AppDbContext.Patrocinadores' is null.");
            }
            var patrocinadorExistente = await _context.Patrocinadores
                .FirstOrDefaultAsync(a => a.Nombre== nuevo.Patrocinador && a.IdHeroe == nuevo.IdHeroe);
            if (patrocinadorExistente != null)
            {
                return Conflict($"El patrocinador {nuevo.Patrocinador} ya está asociado con el heroe de id {nuevo.IdHeroe}");
            }
            var patrocinadorNuevo = new Patrocinador
            {
                Nombre = nuevo.Patrocinador ?? string.Empty,
                IdHeroe = nuevo.IdHeroe??0,
                Origen = nuevo.OrigenDinero ?? "Desconocido",
                Monto = nuevo.Monto??0
            };
            _context.Patrocinadores.Add(patrocinadorNuevo);
            await _context.SaveChangesAsync();
            return Ok(patrocinadorNuevo);
        }

        // DELETE: api/Patrocinadores/BorrarPatrocinador/5
        [HttpDelete("BorrarPatrocinador/{idpatrocinador}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatrocinador(int idpatrocinador)
        {
            if (_context.Patrocinadores == null)
            {
                return NotFound("No existen registros");
            }
            if (idpatrocinador <= 0)
            {
                return BadRequest("El id del patrocinador no puede ser 0 o menor a este");
            }
            var patrocinador = await _context.Patrocinadores.Where(a => a.IdPatrocinador == idpatrocinador).FirstAsync();
            if (patrocinador == null)
            {
                return NotFound("No se encontró el patrocinador");
            }
            _context.Patrocinadores.Remove(patrocinador);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PatrocinadorExists(int id)
        {
            return (_context.Patrocinadores?.Any(e => e.IdPatrocinador == id)).GetValueOrDefault();
        }

    }
}

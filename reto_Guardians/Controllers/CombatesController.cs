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
    public class CombatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CombatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Combates
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CombateDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CombateDTO>>> GetCombates()
        {
            try
            {
                var luchas = await _context.Combates.Select(a => new CombateDTO
                {
                    IdCombate = a.IdCombate,
                    Lugar = a.Lugar,
                    Fecha = a.Fecha,
                    IdHeroe = a.IdHeroe,
                    Heroe = a.IdHeroeNavigation.Alias,
                    IdVillano = a.IdVillano,
                    Villano = a.IdVillanoNavigation.Alias,
                    Resultado = a.Resultado
                }).ToListAsync();

                if (luchas == null || luchas.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }
                return Ok(luchas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Combates/BuscarCombateId/1
        [HttpGet("BuscarCombateId/{idcombate}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CombateDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CombateDTO>> GetCombate(int idcombate)
        {
            try
            {
                if (idcombate <= 0)
                {
                    return BadRequest("El id del combate no puede ser 0 o menor a este");
                }
                var lucha = await _context.Combates.FindAsync(idcombate);
                if (lucha == null)
                {
                    return NotFound($"No existe el evento con id {idcombate}");
                }
                var evento = new CombateDTO
                {
                    IdCombate = lucha.IdCombate,
                    Lugar = lucha.Lugar,
                    Fecha = lucha.Fecha,
                    IdHeroe = lucha.IdHeroe,
                    Heroe = lucha.IdHeroeNavigation.Alias,
                    IdVillano = lucha.IdVillano,
                    Villano = lucha.IdVillanoNavigation.Alias,
                    Resultado = lucha.Resultado

                };
                return Ok(lucha);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Combates/VillanoMasCombates/Batman
        [HttpGet("VillanoMasCombates/{aliasHeroe}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CombateDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CombateDTO>> VillanoConMasCombates(string aliasHeroe)
        {
            try {
                if (string.IsNullOrEmpty(aliasHeroe))
                {
                    return BadRequest("Debe proporcionarse un alias para la búsqueda");
                }
                var heroe = await _context.Heroes
                .Include(h => h.Combates)
                .ThenInclude(c => c.IdVillanoNavigation)
                .SingleOrDefaultAsync(h => h.Alias == aliasHeroe);

                if (heroe == null)
                {
                    return NotFound($"No existe el héroe {aliasHeroe}.");
                }

                var villanoConMasCombates = heroe.Combates
                    .GroupBy(c => c.IdVillanoNavigation)
                    .OrderByDescending(group => group.Count())
                    .FirstOrDefault();

                if (villanoConMasCombates == null)
                {
                    return NotFound($"{aliasHeroe} no ha tenido combates con ningún villano.");
                }

                var combates = villanoConMasCombates.Select(c => new CombateDTO
                {
                    IdCombate = c.IdCombate,
                    Fecha = c.Fecha,
                    Lugar = c.Lugar,
                    Resultado = c.Resultado,
                    IdHeroe = c.IdHeroe,
                    Heroe = c.IdHeroeNavigation.Alias,
                    IdVillano = c.IdVillano,
                    Villano = c.IdVillanoNavigation.Alias
                }).ToList();
                return Ok(combates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            
        }

        // PUT: api/Combates/ModificarCombate/5
        [HttpPut("ModificarCombate/{idcombate}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCombate(int idcombate, [FromBody] CombateDTO combateUpdate)
        {
            if (idcombate <= 0)
                {
                    return BadRequest("El id del combate no puede ser 0 o menor a este");
                }
            var existingCombate = await _context.Combates.FindAsync(idcombate);
            if (existingCombate == null)
            {
                return NotFound($"No se encontró el evento con el id {idcombate}");
            }
            if (!string.IsNullOrEmpty(combateUpdate.Lugar))
            {
                existingCombate.Lugar = combateUpdate.Lugar;
            }
            if (!string.IsNullOrEmpty(combateUpdate.Resultado))
            {
                existingCombate.Resultado = combateUpdate.Resultado;
            }
            if (combateUpdate.Fecha != null && combateUpdate.Fecha != DateTime.MinValue)
            {
                existingCombate.Fecha = combateUpdate.Fecha.Value;
            }
            if (combateUpdate.IdHeroe != null && combateUpdate.IdHeroe != 0)
            {
                existingCombate.IdHeroe = combateUpdate.IdHeroe.Value;
            }
            if (combateUpdate.IdVillano != null && combateUpdate.IdVillano != 0)
            {
                existingCombate.IdVillano = combateUpdate.IdVillano.Value;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CombateExists(idcombate))
                {
                    return NotFound("No existe el combate con el id proporcionado");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Combates/CrearCombate
        [HttpPost("CrearCombate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CombateDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Combate>> PostCombate([FromBody] CombateDTO nuevo)
        {
            if (_context.Combates == null)
            {
                return Problem("Entity set 'AppDbContext.Combates' is null.");
            }
            var combateExistente = await _context.Combates
                .FirstOrDefaultAsync(a => a.IdVillano==nuevo.IdVillano && a.IdHeroe ==nuevo.IdHeroe && a.Fecha == nuevo.Fecha);

            if (combateExistente != null)
            {
                return Conflict("Ya existe un combate registrado de los participantes en la fecha establecida");
            }
            var combateNuevo = new Combate
            {
                Lugar = nuevo.Lugar ?? string.Empty,
                Resultado = nuevo.Resultado ?? string.Empty,
                IdHeroe = nuevo.IdHeroe ??  0,
                IdVillano = nuevo.IdVillano ?? 0,
                Fecha = nuevo.Fecha ?? DateTime.Now,
            };
            _context.Combates.Add(combateNuevo);
            await _context.SaveChangesAsync();
            return Ok(combateNuevo);
        }

        // DELETE: api/Combates/BorrarCombate/5
        [HttpDelete("BorrarCombate/{idcombate}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCombate(int idcombate)
        {
            try {
                if (_context.Combates == null)
                {
                    return NotFound("No existen registros de combates");
                }
                if (idcombate <= 0)
                {
                    return BadRequest("El id del combate no puede ser 0 o menor a este");
                }
                var combate = await _context.Combates.Where(a => a.IdCombate == idcombate).FirstAsync();
                if (combate == null)
                {
                    return NotFound($"No se encontró el combate con id {idcombate}");
                }
                _context.Combates.Remove(combate);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            
        }

        private bool CombateExists(int id)
        {
            return (_context.Combates?.Any(e => e.IdCombate == id)).GetValueOrDefault();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly WebApiContext _context;

        public RolesController(WebApiContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            var roles = await (from r in _context.Roles select r).ToListAsync();
            if (roles == null)
                return NotFound();
            else
                return Ok(roles);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Roles>> GetRoles(int id)
        {
            var roles = await (from r in _context.Roles where r.RoleId == id select r).FirstOrDefaultAsync();

            if (roles == null)
            {
                return NotFound();
            }

            return Ok(roles);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutRoles(int id,[FromForm] Roles roles)
        {
            if (id != roles.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(roles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolesExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(409)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Roles>> PostRoles([FromForm] Roles roles)
        {
            var existRole = await (from r in _context.Roles where r.RoleName == roles.RoleName select r).FirstOrDefaultAsync();
            if (existRole != null)
                return Conflict();
            else
            {
                _context.Roles.Add(roles);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRoles", new { id = roles.RoleId }, roles);
            } 
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<Roles>> DeleteRoles(int id)
        {
            var roles = await _context.Roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(roles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolesExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}

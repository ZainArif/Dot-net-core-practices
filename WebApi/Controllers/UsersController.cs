using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repository.IRepository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebApiContext _context;
        private readonly IUserRepository _userRepo;

        public UsersController(WebApiContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUser()
        {
            //var users = await _context.User.ToListAsync();
            //var users = await (from u in _context.User join r in _context.Roles on u.RoleId equals r.RoleId select new {u.Id,u.Name,r.RoleId,r.RoleName}).ToListAsync();

            var users = await _userRepo.GetUsers();
            if (users == null)
                return NotFound();
            else
                return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUser(long id)
        {
            //var user = await _context.User.FindAsync(id);
            //var user = await (from u in _context.User join r in _context.Roles on u.RoleId equals r.RoleId where u.Id == id select new { u.Id, u.Name, r.RoleId, r.RoleName }).FirstOrDefaultAsync();

            var user = await _userRepo.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutUser(long id,[FromForm] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            //_context.Entry(user).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!UserExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            var existUser = await _userRepo.UserExists(user.Id);
            if (!existUser)
                return NotFound();

            var userUpdated = await _userRepo.UpdateUser(user);
            if (userUpdated)
                return NoContent();
            else
                return StatusCode(500);
        }

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(409)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> PostUser([FromForm] User user)
        {
            //var existUser = await (from u in _context.User where u.Name == user.Name select u).FirstOrDefaultAsync();
            var existUser = await _userRepo.UserExists(user.Name);
            if (existUser)
                return Conflict();
            //else
            //{
            //    _context.User.Add(user);
            //    await _context.SaveChangesAsync();
            //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
            //}   
            var userCraeted = await _userRepo.CreateUser(user);
            if (userCraeted)
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            else
                return StatusCode(500);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteUser(long id)
        {
            //var user = await _context.User.FindAsync(id);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //_context.User.Remove(user);
            //await _context.SaveChangesAsync();

            //return NoContent();

            var existUser = await _userRepo.UserExists(id);
            if (!existUser)
                return NotFound();

            var user = await _userRepo.GetIndUser(id);

            var userUpdated = await _userRepo.DeleteUser(user);

            if (userUpdated)
                return NoContent();
            else
                return StatusCode(500);

            
        }

        //private bool UserExists(long id)
        //{
        //    return _context.User.Any(e => e.Id == id);
        //}
    }
}

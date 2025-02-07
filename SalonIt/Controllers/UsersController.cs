using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonIt.Models;

namespace SalonIt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SalonItContext _context;

        public UsersController(SalonItContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users (Signup)
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (user.Role?.ToLower() == "admin")
            {
                var admin = new Admin
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Contact = user.Contact,
                    Email = user.Email,
                    Password = user.Password
                };
                _context.Admins.Add(admin);
            }
            else if (user.Role?.ToLower() == "owner")
            {
                var owner = new Owner
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Contact = user.Contact,
                    Email = user.Email,
                    Password = user.Password
                };
                _context.Owners.Add(owner);
            }

            await _context.SaveChangesAsync(); // Ensure changes are saved

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new
            {   
                user.FirstName,
                user.LastName,
                user.UserId,
                user.Email,
                user.Role
            });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}

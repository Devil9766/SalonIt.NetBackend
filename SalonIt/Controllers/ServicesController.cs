using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonIt.Models;

namespace SalonIt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly SalonItContext _context;

        public ServicesController(SalonItContext context)
        {
            _context = context;
        }

        // ✅ GET ALL SERVICES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }

        // ✅ GET SERVICE BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            return Ok(service);
        }

        // ✅ GET SERVICES BY SALON ID
        [HttpGet("salon/{salonId}")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServicesBySalon(int salonId)
        {
            var services = await _context.Services
                .Where(s => s.SalonId == salonId)
                .ToListAsync();

            return Ok(services); // Return empty list if no services exist
        }

        // ✅ CREATE NEW SERVICE
        [HttpPost]
        public async Task<ActionResult<Service>> PostService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest(new { message = "Invalid service data" });
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
        }

        // ✅ UPDATE SERVICE
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, [FromBody] Service service)
        {
            if (id != service.ServiceId)
            {
                return BadRequest(new { message = "Service ID mismatch" });
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return NotFound(new { message = "Service not found" });
                }
                throw;
            }

            return NoContent();
        }

        // ✅ DELETE SERVICE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound(new { message = "Service not found" });
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service deleted successfully" });
        }

        // ✅ CHECK IF SERVICE EXISTS
        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}

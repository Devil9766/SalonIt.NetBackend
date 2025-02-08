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
    public class AppointmentsController : ControllerBase
    {
        private readonly SalonItContext _context;

        public AppointmentsController(SalonItContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/Appointments
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Appointment>>> PostAppointments([FromBody] AppointmentRequest request)
        {
            if (request.Services == null || request.Services.Count == 0)
            {
                return BadRequest("At least one service is required.");
            }

            var appointments = request.Services.Select(serviceId => new Appointment
            {
                UserId = request.UserId,
                SalonId = request.SalonId,
                ServiceId = serviceId,
                AppointmentDate = DateTime.UtcNow,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _context.Appointments.AddRange(appointments);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointments), appointments);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Find related payments and remove them first
            var relatedPayments = _context.Payments.Where(p => p.AppointmentId == id);
            _context.Payments.RemoveRange(relatedPayments);

            _context.Appointments.Remove(appointment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Error deleting appointment. Ensure related records are deleted first.", details = ex.Message });
            }

            return NoContent();
        }


        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
    public class AppointmentRequest
    {
        public int UserId { get; set; }
        public int SalonId { get; set; }
        public List<int> Services { get; set; }
    }
}

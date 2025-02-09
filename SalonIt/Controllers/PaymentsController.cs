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
    public class PaymentsController : ControllerBase
    {
        private readonly SalonItContext _context;

        public PaymentsController(SalonItContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/Payments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<List<Payment>>> PostPayment([FromBody] PaymentRequestDto paymentDto)
        {
            if (paymentDto == null || paymentDto.UserId == null || paymentDto.Appointments == null || !paymentDto.Appointments.Any())
            {
                return BadRequest("Invalid payment data.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                List<Payment> createdPayments = new();

                foreach (var appointment in paymentDto.Appointments)
                {
                    if (appointment.AppointmentId == null || appointment.Price == null)
                        continue; // Skip if data is incomplete

                    // Create a new Payment record for this appointment
                    var payment = new Payment
                    {
                        UserId = paymentDto.UserId,
                        AppointmentId = appointment.AppointmentId,
                        Amount = appointment.Price, // Use the price sent from the frontend
                        PaymentDate = DateTime.UtcNow,
                        PaymentMethod = paymentDto.Method ?? "Unknown"
                    };

                    _context.Payments.Add(payment);
                    createdPayments.Add(payment);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction("GetPayments", createdPayments);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while processing the payment.");
            }
        }


        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
    public class PaymentRequestDto
    {
        public int? UserId { get; set; }
        public string? Method { get; set; }
        public List<AppointmentPaymentDto> Appointments { get; set; } = new();
    }

    public class AppointmentPaymentDto
    {
        public int? AppointmentId { get; set; }
        public decimal? Price { get; set; }
        public string? Method { get; set; }
    }

}

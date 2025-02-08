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
    public class ReviewfeedbacksController : ControllerBase
    {
        private readonly SalonItContext _context;

        public ReviewfeedbacksController(SalonItContext context)
        {
            _context = context;
        }

        // GET: api/Reviewfeedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reviewfeedback>>> GetReviewfeedbacks()
        {
            return await _context.Reviewfeedbacks.ToListAsync();
        }

        // GET: api/Reviewfeedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reviewfeedback>> GetReviewfeedback(int id)
        {
            var reviewfeedback = await _context.Reviewfeedbacks.FindAsync(id);

            if (reviewfeedback == null)
            {
                return NotFound();
            }

            return reviewfeedback;
        }

        // GET: api/Reviewfeedbacks/salon/{salonId}
        [HttpGet("salon/{salonId}")]
        public async Task<ActionResult<IEnumerable<Reviewfeedback>>> GetReviewsBySalonId(int salonId)
        {
            var reviews = await _context.Reviewfeedbacks
                                        .Where(r => r.SalonId == salonId)
                                        .OrderByDescending(r => r.CreatedAt) // Latest reviews first
                                        .ToListAsync();

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found for this salon." });
            }

            return Ok(reviews);
        }


        // PUT: api/Reviewfeedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReviewfeedback(int id, Reviewfeedback reviewfeedback)
        {
            if (id != reviewfeedback.ReviewId)
            {
                return BadRequest();
            }

            _context.Entry(reviewfeedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewfeedbackExists(id))
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
        //Post : api/Reviewfeedbacks/
        [HttpPost]
        public async Task<ActionResult<Reviewfeedback>> PostReviewfeedback([FromBody] Reviewfeedback reviewfeedback)
        {
            // Check if the request body is null
            if (reviewfeedback == null)
            {
                return BadRequest(new { message = "Invalid request. Review feedback data is required." });
            }

            // Validate ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure required fields are not empty
            if (reviewfeedback.UserId == 0 || reviewfeedback.SalonId == 0 || string.IsNullOrEmpty(reviewfeedback.Feedback))
            {
                return BadRequest(new { message = "User ID, Salon ID, and Feedback are required." });
            }

            // Validate if User and Salon exist in a single query for efficiency
            var userSalonExist = await _context.Users.AnyAsync(u => u.UserId == reviewfeedback.UserId) &&
                                 await _context.Salons.AnyAsync(s => s.SalonId == reviewfeedback.SalonId);

            if (!userSalonExist)
            {
                return BadRequest(new { message = "Invalid User ID or Salon ID." });
            }

            // Ensure rating is between 1 and 5
            if (reviewfeedback.Rating < 1 || reviewfeedback.Rating > 5)
            {
                return BadRequest(new { message = "Rating must be between 1 and 5." });
            }

            // Set createdAt automatically
            reviewfeedback.CreatedAt = DateTime.UtcNow;

            try
            {
                _context.Reviewfeedbacks.Add(reviewfeedback);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetReviewfeedback), new { id = reviewfeedback.ReviewId }, reviewfeedback);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the review feedback.", error = ex.Message });
            }
        }


        // DELETE: api/Reviewfeedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewfeedback(int id)
        {
            var reviewfeedback = await _context.Reviewfeedbacks.FindAsync(id);
            if (reviewfeedback == null)
            {
                return NotFound();
            }

            _context.Reviewfeedbacks.Remove(reviewfeedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewfeedbackExists(int id)
        {
            return _context.Reviewfeedbacks.Any(e => e.ReviewId == id);
        }
    }
}

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

        // POST: api/Reviewfeedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reviewfeedback>> PostReviewfeedback(Reviewfeedback reviewfeedback)
        {
            _context.Reviewfeedbacks.Add(reviewfeedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviewfeedback", new { id = reviewfeedback.ReviewId }, reviewfeedback);
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

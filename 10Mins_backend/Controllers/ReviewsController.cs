using _10Mins_backend.DBContext;
using _10Mins_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10Mins_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context) => _context = context;

        [HttpGet("{courseId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(Guid courseId)
        {
            return await _context.Reviews
                .Where(r => r.CourseId == courseId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }
    }
}

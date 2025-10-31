using _10Mins_backend.DBContext;
using _10Mins_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace _10Mins_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EnrollmentsController(AppDbContext context) => _context = context;

        [Authorize(Roles = "Student")]
        [HttpPost("{courseId}")]
        public async Task<IActionResult> Enroll(Guid courseId)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null) return Unauthorized();

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.UserId == Guid.Parse(userId) && e.CourseId == courseId);

            if (alreadyEnrolled) return BadRequest("Already enrolled.");

            var enrollment = new Enrollment
            {
                UserId = Guid.Parse(userId),
                CourseId = courseId,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Enrolled successfully." });
        }

        [Authorize]
        [HttpGet("my-courses")]
        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null) return Unauthorized();

            var enrolled = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == Guid.Parse(userId))
                .ToListAsync();

            return Ok(enrolled);
        }
    }

}

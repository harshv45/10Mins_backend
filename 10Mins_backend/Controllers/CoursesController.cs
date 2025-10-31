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
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.Include(c => c.Instructor).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(Guid id)
        {
            var course = await _context.Courses.Include(c => c.Lessons)
                                               .Include(c => c.Instructor)
                                               .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return course;
        }

        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null) return Unauthorized("Invalid token.");

            course.InstructorId = Guid.Parse(userId);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, Course updated)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Title = updated.Title;
            course.Description = updated.Description;
            course.Price = updated.Price;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Instructor,Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var userId = User.FindFirst("id")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            if (role != "Admin" && course.InstructorId.ToString() != userId)
                return Forbid("You can only delete your own courses.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

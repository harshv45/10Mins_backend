using _10Mins_backend.DBContext;
using _10Mins_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10Mins_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LessonsController(AppDbContext context) => _context = context;

        [HttpGet("{courseId}")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons(Guid courseId)
        {
            return await _context.Lessons.Where(l => l.CourseId == courseId).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return Ok(lesson);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _10Mins_backend.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Instructor")]
        public Guid InstructorId { get; set; }

        public User? Instructor { get; set; }

        public decimal Price { get; set; }

        public ICollection<Lesson>? Lessons { get; set; }
    }
}

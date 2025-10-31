using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _10Mins_backend.Models
{
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        public Course? Course { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string VideoUrl { get; set; } = string.Empty;
    }
}

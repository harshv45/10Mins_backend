using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _10Mins_backend.Models
{
    public class Enrollment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    }
}

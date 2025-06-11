using System.ComponentModel.DataAnnotations;

namespace LevelUp.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DueTo { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LevelUp.Models
{
    public class UserTask
    {
        [Key]
        public int UserTaskId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        public DateTime? CompletionDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
    }
}

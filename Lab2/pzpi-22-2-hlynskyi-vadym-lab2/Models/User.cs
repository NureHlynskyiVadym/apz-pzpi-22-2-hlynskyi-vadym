using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LevelUp.Models
{
    public class User
    {
         
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public int Level { get; set; } = 1;
        public int XP { get; set; } = 0;
        public int XPToNextLevel { get; set; } = 100;
        public int Points { get; set; } = 0;
        public bool Admin { get; set; } = false;

        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }

        public ICollection<UserReward> UserRewards { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}

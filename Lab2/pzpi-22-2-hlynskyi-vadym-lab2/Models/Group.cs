using System.ComponentModel.DataAnnotations;

namespace LevelUp.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int MaxMembers { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
    }
}

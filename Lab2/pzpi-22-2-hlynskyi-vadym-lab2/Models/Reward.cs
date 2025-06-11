using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LevelUp.Models
{
    public class Reward
    {
        [Key]
        public int RewardId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public int Price { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<UserReward> UserRewards { get; set; }
    }
}

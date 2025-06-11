using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LevelUp.Models
{
    public class UserReward
    {
        [Key]
        public int UserRewardId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int RewardId { get; set; }
        [ForeignKey("RewardId")]
        public Reward Reward { get; set; }
    }
}

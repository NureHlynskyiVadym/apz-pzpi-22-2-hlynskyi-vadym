using LevelUp.Data;
using LevelUp.Interfaces;
using LevelUp.Models;
using System.Collections.Generic;
using System.Linq;

namespace LevelUp.Repositories
{
    public class RewardRepository : IRewardRepository
    {
        private readonly DataContext _context;

        public RewardRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Reward> GetRewards()
        {
            return _context.Rewards.ToList();
        }

        public Reward GetRewardById(int id)
        {
            return _context.Rewards.Find(id);
        }

        public bool AddReward(Reward reward)
        {
            _context.Rewards.Add(reward);
            return Save();
        }

        public bool UpdateReward(Reward reward)
        {
            _context.Rewards.Update(reward);
            return Save();
        }

        public ICollection<Reward> GetRewardsByUserId(int userId)
        {
            return _context.UserRewards
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Reward)
                .ToList();
        }


        public bool ClaimReward(int userId, int rewardId)
        {
            var reward = _context.Rewards.Find(rewardId);
            var user = _context.Users.Find(userId);

            if (reward == null || user == null) return false;

            // Перевірити, чи вже отримав користувач цю нагороду
            if (_context.UserRewards.Any(ur => ur.UserId == userId && ur.RewardId == rewardId))
                return false;

            // Створити запис про отримання нагороди
            var userReward = new UserReward
            {
                UserId = userId,
                RewardId = rewardId
            };

            _context.UserRewards.Add(userReward);
            return Save();
        }

        public bool DeleteReward(int id)
        {
            var reward = GetRewardById(id);
            if (reward == null) return false;

            _context.Rewards.Remove(reward);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

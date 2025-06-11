using LevelUp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    public class LevelController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public LevelController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        //Формула для розрахунку кількосту хр для наступного рівня
        public int CalculateXpForNextLevel(int currentLevel, int baseXp = 100, double growthFactor = 1.2, int progressBonus = 20)
        {
            if (currentLevel <= 0)
                throw new ArgumentException("Level must be greater than 0.");

            double xpForNextLevel = baseXp * Math.Pow(currentLevel, growthFactor)
                                  + (progressBonus * currentLevel * Math.Log(currentLevel + 1));

            return (int)Math.Ceiling(xpForNextLevel);
        }

        [HttpPost("level-up/{userId}")]
        public async Task<IActionResult> LevelUp(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            if (user == null)
                return NotFound("User not found.");

            if (user.XP >= user.XPToNextLevel)
            {
                user.XP -= user.XPToNextLevel;

                user.Level++;

                user.XPToNextLevel = CalculateXpForNextLevel(user.Level);

                _userRepository.UpdateUser(user);

                return Ok(new
                {
                    Message = "Level up successful!",
                    NewLevel = user.Level,
                    RemainingXP = user.XP,
                    XPToNextLevel = user.XPToNextLevel
                });
            }

            return BadRequest(new
            {
                Message = "Not enough XP to level up.",
                CurrentXP = user.XP,
                XPToNextLevel = user.XPToNextLevel
            });
        }
    }
}

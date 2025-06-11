using LevelUp.Interfaces;
using LevelUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ITaskRepository _taskRepository;

        public AdminController(IUserRepository userRepository, IGroupRepository groupRepository, ITaskRepository taskRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _taskRepository = taskRepository;
        }

        

        [HttpPut("users/{id}/role")]
        public IActionResult UpdateUserRole(int id)
        {

            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound(new { success = false, message = $"User with ID {id} not found." });

            user.Admin = !user.Admin;
            if (!_userRepository.UpdateUser(user))
                return StatusCode(500, new { success = false, message = "Error updating user role." });

            return Ok(new { success = true, data = user.Admin });
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {

            if (!_userRepository.DeleteUser(id))
                return NotFound(new { success = false, message = $"User with ID {id} not found." });

            return Ok(new { success = true });
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {

            var users = _userRepository.GetUsers();
            return Ok(new { success = true, data = users });
        }

        [HttpDelete("groups/{id}")]
        public IActionResult DeleteGroup(int id)
        {

            if (!_groupRepository.DeleteGroup(id))
                return NotFound(new { success = false, message = $"Group with ID {id} not found." });

            return Ok(new { success = true });
        }

        [HttpGet("statistics/users")]
        public IActionResult GetUserStatistics()
        {

            var users = _userRepository.GetUsers();
            var statistics = new
            {
                TotalUsers = users.Count,
                Admins = users.Count(u => u.Admin == true)
            };

            return Ok(new { success = true, data = statistics });
        }

        [HttpDelete("goals/{id}")]
        public IActionResult DeleteTask(int id)
        {

            if (!_taskRepository.DeleteTask(id))
                return NotFound(new { success = false, message = $"Goal with ID {id} not found." });

            return Ok(new { success = true });
        }
    }
}
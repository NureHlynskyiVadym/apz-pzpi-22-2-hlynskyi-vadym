using LevelUp.Interfaces;
using LevelUp.Models;
using LevelUp.Models.API;
using LevelUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly JwtService _jwtService;    

        public UsersController(IUserRepository userRepository, IGroupRepository groupRepository, JwtService jwtService)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("User cannot be null");

            if (!_userRepository.AddUser(user))
                return StatusCode(500, "Error adding the user");

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            if (!_userRepository.DeleteUser(id))
                return NotFound($"User with ID {id} not found.");

            return NoContent();
        }

        [HttpPut("{id}/name")]
        public IActionResult UpdateUserName(int id, [FromBody] string newName)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.Name = newName;
            if (!_userRepository.UpdateUser(user))
                return StatusCode(500, "Error updating the user's name.");

            return Ok(user);
        }

        [HttpPut("{id}/remove-group")]
        public IActionResult RemoveUserFromGroup(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.GroupId = null;
            if (!_userRepository.UpdateUser(user))
                return StatusCode(500, "Error removing the user from the group.");

            return Ok(user);
        }

        [HttpPut("{id}/add-points")]
        public IActionResult AddPointsToUser(int id, [FromBody] int points)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.Points += points;
            if (!_userRepository.UpdateUser(user))
                return StatusCode(500, "Error adding points to the user.");

            return Ok(user);
        }

        [HttpPut("{id}/level")]
        public IActionResult UpdateUserLevel(int id, [FromBody] int level)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            user.Level = level;
            if (!_userRepository.UpdateUser(user))
                return StatusCode(500, "Error updating the user's level.");

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<LoginResponseModel>>Login(LoginRequestModel request)
        {
            var result = await _jwtService.Authenticate(request);
            if(result is null)
                return Unauthorized();
            return Ok(result);
        }
    



        [HttpGet("{id}/groups")]
        public IActionResult GetUserGroups(int id)
        {
            var groups = _groupRepository.GetGroupsByUserId(id) ?? new List<Group>(); // Перевіряємо на null і ініціалізуємо пустим списком
            return Ok(new { success = true, data = groups });
        }


    }

}
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
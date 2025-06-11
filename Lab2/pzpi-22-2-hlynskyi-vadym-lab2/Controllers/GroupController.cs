using LevelUp.Interfaces;
using LevelUp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetGroupById(int id)
        {
            var group = _groupRepository.GetGroupById(id);
            if (group == null)
                return NotFound(new { success = false, message = $"Group with ID {id} not found." });

            return Ok(new { success = true, data = group });
        }

        [HttpPost]
        public IActionResult CreateGroup([FromBody] Group group)
        {
            if (group == null)
                return BadRequest(new { success = false, message = "Group cannot be null" });

            if (!_groupRepository.AddGroup(group))
                return StatusCode(500, new { success = false, message = "Error creating the group" });

            return CreatedAtAction(nameof(GetGroupById), new { id = group.GroupId }, new { success = true, data = group });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            if (!_groupRepository.DeleteGroup(id))
                return NotFound(new { success = false, message = $"Group with ID {id} not found." });

            return Ok(new { success = true });
        }

        [HttpPut("{id}/name")]
        public IActionResult UpdateGroupName(int id, [FromBody] string newName)
        {
            var group = _groupRepository.GetGroupById(id);
            if (group == null)
                return NotFound(new { success = false, message = $"Group with ID {id} not found." });

            group.Title = newName;
            if (!_groupRepository.UpdateGroup(group))
                return StatusCode(500, new { success = false, message = "Error updating the group's name." });

            return Ok(new { success = true, data = group });
        }

        [HttpPut("{id}/max-members")]
        public IActionResult UpdateGroupMaxMembers(int id, [FromBody] int maxMembers)
        {
            var group = _groupRepository.GetGroupById(id);
            if (group == null)
                return NotFound(new { success = false, message = $"Group with ID {id} not found." });

            group.MaxMembers = maxMembers;
            if (!_groupRepository.UpdateGroup(group))
                return StatusCode(500, new { success = false, message = "Error updating the group's max members." });

            return Ok(new { success = true, data = group });
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetGroupsByUserId(int userId)
        {
            var groups = _groupRepository.GetGroupsByUserId(userId);
            if (groups == null || !groups.Any())
                return NotFound(new { success = false, message = $"No groups found for user with ID {userId}." });

            return Ok(new { success = true, data = groups });
        }

        [HttpGet("{groupId}/users")]
        public IActionResult GetUsersByGroupId(int groupId)
        {
            var users = _groupRepository.GetUsersByGroupId(groupId);
            if (users == null || !users.Any())
                return NotFound(new { success = false, message = $"No users found in group with ID {groupId}." });

            return Ok(new { success = true, data = users });
        }


        [HttpPost("{groupId}/add-user/{userId}")]
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            var group = _groupRepository.GetGroupById(groupId);
            if (group == null)
                return NotFound(new { success = false, message = $"Group with ID {groupId} not found." });

            var userAdded = _groupRepository.AddUserToGroup(groupId, userId);
            if (!userAdded)
                return StatusCode(500, new { success = false, message = "Error adding user to group." });

            return Ok(new { success = true, message = "User successfully added to the group." });
        }

    }
}
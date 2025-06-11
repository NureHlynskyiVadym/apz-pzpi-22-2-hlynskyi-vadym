using LevelUp.Interfaces;
using LevelUp.Models;
using LevelUp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TasksController(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskRepository.GetTaskById(id);
            if (task == null)
                return NotFound(new { success = false, message = $"Task with ID {id} not found." });

            return Ok(new { success = true, data = task });
        }


        //Додавання поінтів та хр за завершення таски
        
        [HttpPost("{taskId}/complete")]
        public IActionResult CompleteTask(int taskId, [FromBody] int userId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == null)
                return NotFound(new { success = false, message = $"Task with ID {taskId} not found." });

            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return NotFound(new { success = false, message = $"User with ID {userId} not found." });

            var taskCompleted = _taskRepository.CompleteTask(taskId, userId);
            if (!taskCompleted)
                return StatusCode(500, new { success = false, message = "Error completing the task." });

            user.XP += task.Points;
            user.Points += task.Points;
            if (!_userRepository.UpdateUser(user))
                return StatusCode(500, new { success = false, message = "Error updating user XP." });

            return Ok(new { success = true, message = "Task marked as completed and XP updated.", data = new { user.XP, task.Points } });
        }
        [HttpPost]
        public IActionResult UpdateTask(int id, [FromBody] LevelUp.Models.Task task)
        {
            if (id != task.TaskId)
                return BadRequest(new { success = false, message = "ID mismatch" });

            var existingTask = _taskRepository.GetTaskById(id);
            if (existingTask == null)
                return NotFound(new { success = false, message = $"Task with ID {id} not found." });

            if (!_taskRepository.UpdateTask(task))
                return StatusCode(500, new { success = false, message = "Error updating the task" });

            return Ok(new { success = true, data = task });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            if (!_taskRepository.DeleteTask(id))
                return NotFound(new { success = false, message = $"Task with ID {id} not found." });

            return Ok(new { success = true });
        }

        [HttpPost("{taskId}/assign/{userId}")]
        public IActionResult AssignTaskToUser(int taskId, int userId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == null)
                return NotFound(new { success = false, message = $"Task with ID {taskId} not found." });

            if (!_taskRepository.AssignTaskToUser(taskId, userId))
                return StatusCode(500, new { success = false, message = "Error assigning task to user." });

            return Ok(new { success = true, message = "Task successfully assigned." });
        }

        [HttpGet("assigned-to/{userId}")]
        public IActionResult GetTasksAssignedToUser(int userId)
        {
            var tasks = _taskRepository.GetTasksAssignedToUser(userId);
            if (tasks == null || !tasks.Any())
                return NotFound(new { success = false, message = $"No tasks assigned to user with ID {userId}." });

            return Ok(new { success = true, data = tasks });
        }

    }
}

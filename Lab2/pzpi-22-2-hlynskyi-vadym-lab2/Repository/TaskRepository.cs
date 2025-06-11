using LevelUp.Data;
using LevelUp.Interfaces;
using LevelUp.Models;
using System.Collections.Generic;
using System.Linq;

namespace LevelUp.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext _context;

        public TaskRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<LevelUp.Models.Task> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        public LevelUp.Models.Task GetTaskById(int id)
        {
            return _context.Tasks.Find(id);
        }

        public bool AddTask(LevelUp.Models.Task task)
        {
            _context.Tasks.Add(task);
            return Save();
        }

        public bool UpdateTask(LevelUp.Models.Task task)
        {
            _context.Tasks.Update(task);
            return Save();
        }

        public bool DeleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            return Save();
        }

        public bool CompleteTask(int taskId, int userId)
        {
            var task = _context.Tasks.Find(taskId);
            var user = _context.Users.Find(userId);

            if (task == null || user == null) return false;

            // Перевірити, чи завдання вже виконано
            if (_context.UserTasks.Any(ut => ut.TaskId == taskId && ut.UserId == userId && ut.Status=="completed"))
                return false;

            // Оновити стан завдання для користувача
            var userTask = _context.UserTasks.FirstOrDefault(ut => ut.TaskId == taskId && ut.UserId == userId);
            if (userTask == null)
            {
               return false;
            }
            else
            {
                userTask.Status = "completed";
                userTask.CompletionDate = DateTime.UtcNow;
                _context.UserTasks.Update(userTask);
            }

            return Save();
        }


        public bool AssignTaskToUser(int taskId, int userId)
        {
            var task = _context.Tasks.Find(taskId);
            var user = _context.Users.Find(userId);

            if (task == null || user == null) return false;

            // Assign the task to the user
            var userTask = new UserTask
            {
                TaskId = taskId,
                UserId = userId
            };

            _context.UserTasks.Add(userTask);
            return Save();
        }
        public ICollection<LevelUp.Models.Task> GetTasksAssignedToUser(int userId)
        {
            return _context.UserTasks
                .Where(ut => ut.UserId == userId)
                .Select(ut => ut.Task)
                .ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

using LevelUp.Data;
using LevelUp.Interfaces;
using LevelUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelUp.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User> GetUsersByGroupId(int groupId)
        {
            return _context.Users
                .Where(u => u.UserGroups.Any(ug => ug.GroupId == groupId))
                .ToList();
        }


        public ICollection<Group> GetGroups()
        {
            return _context.Groups.ToList();
        }

        public ICollection<Group> GetGroupsByUserId(int userId)
        {
            return _context.Groups.Where(g => g.UserGroups.Any(ug => ug.UserId == userId)).ToList();
        }

        public Group GetGroupById(int id)
        {
            return _context.Groups.Find(id);
        }

        public bool AddGroup(Group group)
        {
            _context.Groups.Add(group);
            return Save();
        }

        public bool DeleteGroup(int id)
        {
            var group = GetGroupById(id);
            if (group == null) return false;

            var usersInGroup = _context.Users.Where(u => u.GroupId == id).ToList();
            foreach (var user in usersInGroup)
            {
                user.GroupId = null; // або _context.Users.Remove(user) для повного видалення користувача
            }

            _context.Groups.Remove(group);
            return Save();
        }


        public bool UpdateGroup(Group group)
        {
            _context.Groups.Update(group);
            return Save();
        }

        public bool AddUserToGroup(int groupId, int userId)
        {
            var group = _context.Groups.Find(groupId);
            var user = _context.Users.Find(userId);

            if (group == null || user == null) return false;

            // Перевірка, чи користувач уже в групі
            if (_context.UserGroups.Any(ug => ug.GroupId == groupId && ug.UserId == userId))
                return false;

            // Додайте користувача до групи
            var userGroup = new UserGroup
            {
                GroupId = groupId,
                UserId = userId
            };

            _context.UserGroups.Add(userGroup);
            return Save();
        }


        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
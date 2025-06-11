using LevelUp.Data;
using LevelUp.Models;
using LevelUp.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LevelUp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(p => p.UserId).ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return Save();
        }

        public User Authenticate(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }


        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}

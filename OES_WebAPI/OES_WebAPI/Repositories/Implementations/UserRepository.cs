using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public UserRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdatePassword(User user, string newPassword)
        {
            if (user == null) return;

            user.PasswordHash = newPassword;
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
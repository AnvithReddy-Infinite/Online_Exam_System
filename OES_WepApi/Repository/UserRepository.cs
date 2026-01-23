using OES_WepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using BCrypt.Net;

namespace OES_WepApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineExamSystemEntities _context;

        // Constructor - initialize EF context
        public UserRepository()
        {
            _context = new OnlineExamSystemEntities();
        }

        // Get user by email
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        // Get user by ID
        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        // Add a new user (registration)
        public void AddUser(User user)
        {
            // Set created timestamp if not already set
            if (user.CreatedAt == default)
            {
                user.CreatedAt = DateTime.Now;
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Validate user credentials (login)
        public bool ValidateUser(string email, string password)
        {
            var user = GetUserByEmail(email);

            if (user == null) return false;

            // Compare hashed password using BCrypt
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        // Get all users (for admin reports)
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OES_WepApi.Models;

namespace OES_WepApi.Repository
{
    internal interface IUserRepository
    {
        // Get a user by email (used for checking duplicates and login)
        User GetUserByEmail(string email);

        // Get a user by ID (optional, useful for reports)
        User GetUserById(int userId);

        // Add a new user (used for registration)
        void AddUser(User user);

        // Validate user credentials (for login)
        bool ValidateUser(string email, string password);

        // Get all users (for admin reports)
        IEnumerable<User> GetAllUsers();


    }
}

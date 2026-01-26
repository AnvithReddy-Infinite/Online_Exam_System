using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Models;
using OES_WebAPI.Models;

namespace FinalProject.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetByEmail(string  email);
        User GetById(int id);
        void UpdatePassword(User user, string newPassword);

        void SaveChanges();
    }
}

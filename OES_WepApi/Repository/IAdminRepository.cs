using OES_WepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES_WepApi.Repository
{
    public interface IAdminRepository
    {
        Admin Login(string email, string password);
        bool AddQuestion(Question question, List<Option> options);
        bool RemoveQuestion(int questionId);

        List<User> SearchStudents(int? techId, int? levelId, string state, string city, int? minMarks);
    }
}

using OES_WebAPI.Models;
using OES_WepApi.Models;
using OES_WepApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OES_WepApi.Repository.Interfaces
{
    public interface IAdminRepository
    {
        // Admin login
        Admin Login(string email, string password);

        // Upload questions file (Add Questions)
        string UploadQuestionsFile(HttpPostedFile file,int techId,int levelId);

        // Remove questions file
        string RemoveQuestionsByFile(HttpPostedFile file, int techId, int levelId);

        List<StudentSearchResultDTO> SearchStudents(int? techId, int? levelId, string state, string city, int? minMarks);
    }
}

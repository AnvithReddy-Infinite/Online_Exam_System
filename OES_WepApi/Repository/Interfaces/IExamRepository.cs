using FinalProject.Models;
using OES_WepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Interfaces
{
    public interface IExamRepository
    {
        void AddExam(Exam exam);
        void Update(Exam exam);
        Exam GetById(int examId);

        List<Exam> GetByUserTechLevel(int userId, int techId, int levelId);

        List<Exam> GetInProgressExams(int userId, int techId, int levelId);

        void SaveChanges();
    }
}

using FinalProject.Models;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        List<Question> GetByTechAndLevel(int techId, int levelId);

        // Question GetById(int questionId);

    }
}

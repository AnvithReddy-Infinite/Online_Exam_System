using FinalProject.Models;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Interfaces
{
    public interface IOptionRepository
    {
        List<Option> GetByQuestionId(int questionId);
        IEnumerable<Option> GetByQuestionIds(List<int> questionIds);
        Option GetById(int optionId);
    }
}

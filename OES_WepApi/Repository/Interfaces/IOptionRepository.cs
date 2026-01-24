using FinalProject.Models;
using OES_WepApi.Models;
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
        Option GetById(int optionId);
    }
}

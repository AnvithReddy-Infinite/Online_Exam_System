using FinalProject.Models;
using OES_WepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Interfaces
{
    internal interface IResultRepository
    {
        void Add(Result result);
        List<Result> GetByExamId(int examId);
        void SaveChanges();
    }
}

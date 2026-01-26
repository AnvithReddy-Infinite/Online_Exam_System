using FinalProject.Models;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Repositories.Interfaces
{
    public interface IResultRepository
    {
        void Add(Result result);
        List<Result> GetByExamId(int examId);
        void SaveChanges();
    }
}

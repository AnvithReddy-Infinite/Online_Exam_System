using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Repositories.Implementations
{
    public class ResultRepository : IResultRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public ResultRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }

        public void Add(Result result)
        {
            _context.Results.Add(result);
        }

        public List<Result> GetByExamId(int examId)
        {
            return _context.Results
                .Include("Question")
                .Include("Option")
                .Where(r => r.ExamId == examId)
                .ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
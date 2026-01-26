using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public QuestionRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }
        public List<Question> GetByTechAndLevel(int techId, int levelId)
        {
            return _context.Questions.Where(q => q.TechId == techId && q.LevelId == levelId).ToList();
        }

        //public Question GetById(int questionId)
        //{
        //    return _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
        //}
    }
}
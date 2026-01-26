using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Repositories.Implementations
{
    public class OptionRepository : IOptionRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public OptionRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }
        public Option GetById(int optionId)
        {
            return _context.Options.FirstOrDefault(o => o.OptionId == optionId);
        }

        public IEnumerable<Option> GetByQuestionIds(List<int> questionIds)
        {
            if (questionIds == null || !questionIds.Any())
                return new List<Option>();

            return _context.Options
                           .Where(o => questionIds.Contains(o.QuestionId))
                           .ToList();
        }

        public List<Option> GetByQuestionId(int questionId)
        {
            return _context.Options.Where(o => o.QuestionId == questionId).ToList();
        }
    }
}
using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WepApi.Models;
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
        public List<Option> GetByQuestionId(int questionId)
        {
            return _context.Options.Where(o => o.QuestionId == questionId).ToList();
        }
    }
}
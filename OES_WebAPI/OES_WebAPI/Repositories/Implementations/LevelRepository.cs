using OES_WebAPI.Models;
using OES_WebAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES_WebAPI.Repositories.Implementations
{
    public class LevelRepository : ILevelRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public LevelRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }

        public IEnumerable<Level> GetAll()
        {
            return _context.Levels.ToList();
        }

        public Level GetById(int levelId)
        {
            return _context.Levels.FirstOrDefault(l => l.LevelId == levelId);
        }
    }
}
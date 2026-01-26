using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Repositories.Implementations
{
    public class TechRepository : ITechRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public TechRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }

        public Technology GetById(int id)
        {
            return _context.Technologies.FirstOrDefault(t => t.TechId == id);
        }
    }
}
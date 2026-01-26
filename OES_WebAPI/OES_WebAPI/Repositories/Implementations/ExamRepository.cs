using FinalProject.Models;
using FinalProject.Repositories.Interfaces;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Web;

namespace FinalProject.Repositories.Implementations
{
    public class ExamRepository : IExamRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public ExamRepository(OnlineExamSystemEntities context)
        {
            _context = context;
        }
        public void AddExam(Exam exam)
        {
            _context.Exams.Add(exam);
        }

        public void Update(Exam exam)
        {
            _context.Entry(exam).State = EntityState.Modified;
        }

        public Exam GetById(int examId)
        {
            var user = _context.Exams.Include(e => e.Level).Include(e => e.User).FirstOrDefault(e => e.ExamId == examId);
            return user;
        }

        // Get all exams of a user for a specific tech and level
        public List<Exam> GetByUserTechLevel(int userId, int techId, int levelId)
        {
            return _context.Exams
                .Where(e => e.UserId == userId
                         && e.TechId == techId
                         && e.LevelId == levelId)
                .ToList();
        }

        // Get in-progress exams(CompletedAt = null)
        public List<Exam> GetInProgressExams(int userId, int techId, int levelId)
        {
            return _context.Exams
                .Where(e => e.UserId == userId
                         && e.TechId == techId
                         && e.LevelId == levelId
                         && e.CompletedAt == null)
                .ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
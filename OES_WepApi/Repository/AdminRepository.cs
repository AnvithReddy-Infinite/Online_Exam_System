using OES_WepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES_WepApi.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly OnlineExamSystemEntities db = new OnlineExamSystemEntities();

        public Admin Login(string email, string password)
        {
            return db.Admins.FirstOrDefault(a =>
                a.Email == email && a.PasswordHash == password);
        }

        public bool AddQuestion(Question question, List<Option> options)
        {
            db.Questions.Add(question);
            db.SaveChanges();

            foreach (var option in options)
            {
                option.QuestionId = question.QuestionId;
                db.Options.Add(option);
            }

            db.SaveChanges();
            return true;
        }

        public bool RemoveQuestion(int questionId)
        {
            var question = db.Questions.Find(questionId);
            if (question == null) return false;

            var options = db.Options.Where(o => o.QuestionId == questionId);
            db.Options.RemoveRange(options);
            db.Questions.Remove(question);

            db.SaveChanges();
            return true;
        }

        public List<User> SearchStudents(int? techId, int? levelId, string state, string city, int? minMarks)
        {
            try
            {
                var query = from u in db.Users
                            join e in db.Exams on u.UserId equals e.UserId
                            select new { u, e };

                if (techId.HasValue)
                    query = query.Where(x => x.e.TechId == techId.Value);

                if (levelId.HasValue)
                    query = query.Where(x => x.e.LevelId == levelId.Value);

                if (!string.IsNullOrEmpty(state))
                    query = query.Where(x => x.u.State == state);

                if (!string.IsNullOrEmpty(city))
                    query = query.Where(x => x.u.City == city);

                if (minMarks.HasValue)
                    query = query.Where(x => x.e.Score >= minMarks.Value);

                return query
                    .Select(x => x.u)
                    .Distinct()
                    .ToList();
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}
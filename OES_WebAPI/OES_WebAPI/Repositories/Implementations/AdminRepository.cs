using ExcelDataReader;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic.FileIO;
using OES_WebAPI.Models;
using OES_WepApi.Helpers;
using OES_WepApi.Models;
using OES_WepApi.Models.DTOs;
using OES_WepApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OES_WepApi.Repository.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly OnlineExamSystemEntities _context;

        public AdminRepository()
        {
            _context = new OnlineExamSystemEntities();
        }

        public Admin Login(string email, string password)
        {
            return _context.Admins.FirstOrDefault(a =>
                a.Email == email && a.PasswordHash == password);
        }

        // ADD QUESTIONS (UPLOAD FILE)
        public string UploadQuestionsFile(HttpPostedFile file, int techId, int levelId)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                    return "Please upload a valid CSV file";

                if (!file.FileName.EndsWith(".csv"))
                    return "Only CSV files are allowed";

                using (var parser = new TextFieldParser(file.InputStream))
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.HasFieldsEnclosedInQuotes = true;

                    bool isHeader = true;

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (isHeader)
                        {
                            isHeader = false;
                            continue;
                        }

                        if (fields.Length < 6)
                            continue;

                        var question = new Question
                        {
                            QuestionText = fields[0],
                            TechId = techId,
                            LevelId = levelId,
                            CreatedAt = DateTime.Now
                        };

                        _context.Questions.Add(question);
                        _context.SaveChanges();

                        string correct = fields[5].Trim().ToUpper();

                        _context.Options.AddRange(new[]
                        {
                    new Option { QuestionId = question.QuestionId, OptionText = fields[1], IsCorrect = correct == "A" },
                    new Option { QuestionId = question.QuestionId, OptionText = fields[2], IsCorrect = correct == "B" },
                    new Option { QuestionId = question.QuestionId, OptionText = fields[3], IsCorrect = correct == "C" },
                    new Option { QuestionId = question.QuestionId, OptionText = fields[4], IsCorrect = correct == "D" }
                });

                        _context.SaveChanges();
                    }
                }

                return "Questions uploaded successfully";
            }
            catch(Exception ex) 
            {
                return "CSV Error: " + ex.Message;
            }
        }

        // REMOVE QUESTIONS (DELETE FILE)
        public string RemoveQuestionsByFile(HttpPostedFile file, int techId, int levelId)
        {
            try
            {
                var questionTexts = new List<string>();

                using (var reader = new StreamReader(file.InputStream))
                {
                    bool isHeader = true;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (isHeader)
                        {
                            isHeader = false;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // SAME CSV AS UPLOAD
                        // QuestionText,OptionA,OptionB,OptionC,OptionD,CorrectOption
                        var columns = line.Split(',');

                        var questionText = columns[0].Trim();

                        if (!string.IsNullOrEmpty(questionText))
                            questionTexts.Add(questionText);
                    }
                }

                if (!questionTexts.Any())
                    return "No questions found in CSV file.";

                var questions = _context.Questions
                    .Where(q =>
                        q.TechId == techId &&
                        q.LevelId == levelId &&
                        questionTexts.Contains(q.QuestionText))
                    .ToList();

                if (!questions.Any())
                    return "No matching questions found.";

                var questionIds = questions.Select(q => q.QuestionId).ToList();

                var options = _context.Options
                    .Where(o => questionIds.Contains(o.QuestionId))
                    .ToList();

                _context.Options.RemoveRange(options);
                _context.Questions.RemoveRange(questions);
                _context.SaveChanges();

                return $"{questions.Count} questions removed successfully.";
            }
            catch
            {
                return "Error while removing questions from CSV file.";
            }
        }

        public List<StudentSearchResultDTO> SearchStudents(int? techId, int? levelId, string state, string city, int? minMarks)
        {
            try
            {
                var query =
                from u in _context.Users
                join e in _context.Exams on u.UserId equals e.UserId
                select new StudentSearchResultDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    City = u.City,
                    State = u.State,
                    TechId = e.TechId,
                    LevelId = e.LevelId,
                    Score = e.Score
                };

                if (techId.HasValue)
                    query = query.Where(x => x.TechId == techId.Value);

                if (levelId.HasValue)
                    query = query.Where(x => x.LevelId == levelId.Value);

                if (!string.IsNullOrEmpty(state))
                    query = query.Where(x => x.State == state);

                if (!string.IsNullOrEmpty(city))
                    query = query.Where(x => x.City == city);

                if (minMarks.HasValue)
                    query = query.Where(x => x.Score >= minMarks.Value);

                return query.ToList();
            }
            catch
            {
                return new List<StudentSearchResultDTO>();
            }
        }
    }
}
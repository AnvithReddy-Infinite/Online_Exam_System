using Newtonsoft.Json.Linq;
using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminResultController : Controller
    {
        private readonly string apiBase = "https://localhost:44384/";

        public async Task<ActionResult> Index()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            var results = new List<AdminExamResultViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                // STEP 1: get all users
                var usersRes = await client.GetAsync("api/admin/search-students");
                var usersJson = await usersRes.Content.ReadAsStringAsync();
                var users = JArray.Parse(usersJson);

                foreach (var user in users)
                {
                    int userId = user["UserId"].Value<int>();
                    string userName = user["FullName"].ToString();

                    // STEP 2: get exams per user
                    var examRes = await client.GetAsync($"api/exams/user/{userId}/results");
                    var examJson = await examRes.Content.ReadAsStringAsync();
                    var examObj = JObject.Parse(examJson);

                    if (!(bool)examObj["Success"]) continue;

                    foreach (var exam in examObj["Data"])
                    {
                        results.Add(new AdminExamResultViewModel
                        {
                            StudentName = userName,
                            ExamName = exam["ExamName"]?.ToString(),
                            Technology = exam["Technology"]?.ToString(),
                            Level = exam["Level"]?.ToString(),
                            Marks = $"{exam["Score"]} / {exam["TotalMarks"]}",
                            Status = exam["Result"]?.ToString(),
                            DateTaken = Convert.ToDateTime(exam["DateTaken"])
                                            .ToString("dd-MMM-yyyy")
                        });
                    }
                }
            }

            return View(results);
        }
    }
}

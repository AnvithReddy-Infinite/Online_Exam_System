using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using OnlineExaminationSystem.Models;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminStudentController : Controller
    {
        private readonly string apiBase = "https://localhost:44384/";

        // =========================
        // SEARCH STUDENTS
        // =========================
        public async Task<ActionResult> Index(
            int? techId,
            int? levelId,
            string state,
            string city,
            int? minMarks)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            var model = new AdminStudentViewModel
            {
                Technologies = new System.Collections.Generic.List<SelectListItem>(),
                Levels = new System.Collections.Generic.List<SelectListItem>(),
                Students = new System.Collections.Generic.List<StudentResultViewModel>()
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                // Load Technologies
                var techRes = await client.GetAsync("api/exams/technologies");
                var techObj = JObject.Parse(await techRes.Content.ReadAsStringAsync());

                model.Technologies = techObj["Data"]
                    .Select(t => new SelectListItem
                    {
                        Value = t["TechId"].ToString(),
                        Text = t["Name"].ToString()
                    })
                    .ToList();

                // Load Levels
                var levelRes = await client.GetAsync("api/exams/levels");
                var levelObj = JObject.Parse(await levelRes.Content.ReadAsStringAsync());

                model.Levels = levelObj["Data"]
                    .Select(l => new SelectListItem
                    {
                        Value = l["LevelId"].ToString(),
                        Text = l["LevelName"].ToString()
                    })
                    .ToList();

                // If any filter is used → call search API
                if (techId.HasValue || levelId.HasValue || !string.IsNullOrEmpty(state)
                    || !string.IsNullOrEmpty(city) || minMarks.HasValue)
                {
                    var query = $"api/admin/search-students?" +
                                $"techId={techId}&levelId={levelId}&state={state}&city={city}&minMarks={minMarks}";

                    var res = await client.GetAsync(query);

                    if (res.IsSuccessStatusCode)
                    {
                        var json = await res.Content.ReadAsStringAsync();

                        if (!json.Contains("No students found"))
                        {
                            var arr = JArray.Parse(json);

                            model.Students = arr.Select(s => new StudentResultViewModel
                            {
                                UserId = (int)s["UserId"],
                                FullName = (string)s["FullName"],
                                Email = (string)s["Email"],
                                Mobile = (string)s["Mobile"],
                                City = (string)s["City"],
                                State = (string)s["State"],
                                TechId = (int)s["TechId"],
                                LevelId = (int)s["LevelId"],
                                Score = (int)s["Score"]
                            }).ToList();
                        }
                    }
                }
            }

            return View(model);
        }
    }
}

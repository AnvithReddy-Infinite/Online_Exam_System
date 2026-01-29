using Newtonsoft.Json.Linq;
using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminLevelController : Controller
    {
        private readonly string apiBase = "https://localhost:44384/";

        public async Task<ActionResult> Index()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            var levels = new List<AdminLevelViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                var res = await client.GetAsync("api/exams/levels");
                var json = await res.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);

                foreach (var item in obj["Data"])
                {
                    levels.Add(new AdminLevelViewModel
                    {
                        LevelId = item["LevelId"].Value<int>(),
                        LevelName = item["LevelName"].ToString(),
                        PassMarks = item["PassMarks"].Value<int>()
                    });
                }
            }

            return View(levels);
        }
    }
}

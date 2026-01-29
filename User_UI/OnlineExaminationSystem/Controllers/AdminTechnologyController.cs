using Newtonsoft.Json.Linq;
using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminTechnologyController : Controller
    {
        private readonly string apiBase = "https://localhost:44384/";

        public async Task<ActionResult> Index()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            var technologies = new List<AdminTechnologyViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                var res = await client.GetAsync("api/exams/technologies");
                var json = await res.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);

                foreach (var item in obj["Data"])
                {
                    technologies.Add(new AdminTechnologyViewModel
                    {
                        TechId = item["TechId"].Value<int>(),
                        Name = item["Name"].ToString()
                    });
                }
            }

            return View(technologies);
        }
    }
}

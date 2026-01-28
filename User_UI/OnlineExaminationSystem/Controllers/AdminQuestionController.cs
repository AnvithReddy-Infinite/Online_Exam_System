using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using OnlineExaminationSystem.Models;
using System.Net.Http.Headers;
using System.IO;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminQuestionController : Controller
    {
        private readonly string apiBase = "https://localhost:44384/";

        // =========================
        // GET: Questions Page
        // =========================
        public async Task<ActionResult> Index()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            var model = new AdminQuestionViewModel
            {
                Technologies = new System.Collections.Generic.List<SelectListItem>(),
                Levels = new System.Collections.Generic.List<SelectListItem>()
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                // Load Technologies
                var techRes = await client.GetAsync("api/exams/technologies");
                var techJson = await techRes.Content.ReadAsStringAsync();
                var techObj = JObject.Parse(techJson);

                model.Technologies = techObj["Data"]
                    .Select(t => new SelectListItem
                    {
                        Value = t["TechId"].ToString(),
                        Text = t["Name"].ToString()
                    })
                    .ToList();

                // Load Levels
                var levelRes = await client.GetAsync("api/exams/levels");
                var levelJson = await levelRes.Content.ReadAsStringAsync();
                var levelObj = JObject.Parse(levelJson);

                model.Levels = levelObj["Data"]
                    .Select(l => new SelectListItem
                    {
                        Value = l["LevelId"].ToString(),
                        Text = l["LevelName"].ToString()
                    })
                    .ToList();
            }

            return View(model);
        }

        // =========================
        // ADD QUESTIONS
        // =========================
        [HttpPost]
        public async Task<ActionResult> AddQuestions(AdminQuestionViewModel model)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            if (model.File == null || model.File.ContentLength == 0)
            {
                TempData["Error"] = "Please upload a CSV file.";
                return RedirectToAction("Index");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                var content = new MultipartFormDataContent();

                var fileContent = new StreamContent(model.File.InputStream);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = Path.GetFileName(model.File.FileName)
                };

                content.Add(fileContent);

                var response = await client.PostAsync(
                    $"api/admin/upload-questions?techId={model.TechId}&levelId={model.LevelId}",
                    content
                );

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Questions uploaded successfully.";
                else
                    TempData["Error"] = "Failed to upload questions.";
            }

            return RedirectToAction("Index");
        }

        // =========================
        // REMOVE QUESTIONS
        // =========================
        [HttpPost]
        public async Task<ActionResult> RemoveQuestions(AdminQuestionViewModel model)
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            if (model.File == null || model.File.ContentLength == 0)
            {
                TempData["Error"] = "Please upload a CSV file.";
                return RedirectToAction("Index");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                var content = new MultipartFormDataContent();

                var fileContent = new StreamContent(model.File.InputStream);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = Path.GetFileName(model.File.FileName)
                };

                content.Add(fileContent);

                var response = await client.PostAsync(
                    $"api/admin/remove-questions-file?techId={model.TechId}&levelId={model.LevelId}",
                    content
                );

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Questions removed successfully.";
                else
                    TempData["Error"] = "Failed to remove questions.";
            }

            return RedirectToAction("Index");
        }
    }
}

using Newtonsoft.Json;
using OnlineExaminationSystem.Common;
using OnlineExaminationSystem.Models.Exam;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class ExamController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44384/api/exams/";

        // ---------- START EXAM ----------
        [HttpGet]
        public ActionResult StartExam()
        {
            if (Session["UserId"] == null)
            {
                TempData["AuthRequired"] = true;
            }

            PopulateDropdowns();
            return View(new StartExamDTO());
        }

        [HttpPost]
        public async Task<ActionResult> StartExam(StartExamDTO model)
        {
            if (Session["UserId"] == null)
            {
                TempData["AuthRequired"] = true;
                PopulateDropdowns();
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }

            model.UserId = (int)Session["UserId"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsJsonAsync("start", model);
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to reach server.");
                    PopulateDropdowns();
                    return View(model);
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<StartExamResponseDTO>>(content);

                if (apiResponse == null)
                {
                    ModelState.AddModelError("", "Unable to start exam.");
                    PopulateDropdowns();
                    return View(model);
                }

                // API may return a "resuming in-progress exam" message
                // We treat both new and resumed exams the same
                if (!apiResponse.Success && apiResponse.Data == null)
                {
                    ModelState.AddModelError("", apiResponse.Message ?? "Unable to start exam.");
                    PopulateDropdowns();
                    return View(model);
                }

                // Redirect to questions page for both new and resumed exams
                return RedirectToAction("GetQuestions", new { examId = apiResponse.Data.ExamId });
            }
        }


        // ---------- GET QUESTIONS ----------
        [HttpGet]
        public async Task<ActionResult> GetQuestions(int examId)
        {
            int userId = (int)Session["UserId"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync($"{examId}/questions?userId={userId}");
                }
                catch
                {
                    TempData["ErrorMessage"] = "Unable to reach server.";
                    return RedirectToAction("Home", "User");
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<QuestionDTO>>>(content);

                if (apiResponse == null || !apiResponse.Success)
                {
                    TempData["ErrorMessage"] = apiResponse?.Message ?? "Failed to fetch questions.";
                    return RedirectToAction("Home", "User");
                }

                var model = new SubmitExamDTO
                {
                    ExamId = examId,
                    UserId = userId,
                    Answers = new List<AnswerDTO>()
                };

                foreach (var q in apiResponse.Data)
                    model.Answers.Add(new AnswerDTO { QuestionId = q.QuestionId });

                ViewBag.Questions = apiResponse.Data;
                return View(model);
            }
        }

        // ---------- SUBMIT EXAM ----------
        [HttpPost]
        public async Task<ActionResult> SubmitExam(SubmitExamDTO model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsJsonAsync("submit", model);
                }
                catch
                {
                    TempData["ErrorMessage"] = "Unable to reach server.";
                    return RedirectToAction("Home", "User");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<SubmitExamResponseDTO>>(responseContent);

                if (apiResponse == null || !apiResponse.Success)
                {
                    TempData["ErrorMessage"] = apiResponse?.Message ?? "Failed to submit exam.";
                    return RedirectToAction("Home", "User");
                }

                // Use a view that expects SubmitExamResponseDTO
                return View("ExamResult", apiResponse.Data);
            }
        }


        // ---------- VIEW RESULT ----------
        [HttpGet]
        public async Task<ActionResult> ViewResult(int examId)
        {
            int userId = (int)Session["UserId"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync($"{examId}/result?userId={userId}");
                }
                catch
                {
                    TempData["ErrorMessage"] = "Unable to reach server.";
                    return RedirectToAction("Home", "User");
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ExamResultDTO>>(content);

                if (apiResponse == null || !apiResponse.Success)
                {
                    TempData["ErrorMessage"] = apiResponse?.Message ?? "Failed to fetch exam result.";
                    return RedirectToAction("Home", "User");
                }

                return View("ExamResult", apiResponse.Data);
            }
        }

        // ---------- HELPER TO POPULATE DROPDOWNS ----------
        private void PopulateDropdowns()
        {
            ViewBag.Techs = new List<dynamic> {
                new { Id = 1, Name = "C#" },
                new { Id = 2, Name = "Java" },
                new { Id = 3, Name = "Python" }
            };

            ViewBag.Levels = new List<dynamic> {
                new { Id = 1, LevelName = "Beginner" },
                new { Id = 2, LevelName = "Intermediate" },
                new { Id = 3, LevelName = "Advanced" }
            };
        }

        public async Task<ActionResult> Report(int? userId)
        {
            if (!userId.HasValue)
                userId = (int)Session["UserId"];

            if (userId == null)
                return RedirectToAction("Login", "Account"); // handle not-logged-in users

            List<UserExamReportDTO> examReports = new List<UserExamReportDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"user/{userId}/results");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<UserExamReportDTO>>>(jsonString);

                    if (apiResponse != null && apiResponse.Success)
                    {
                        examReports = apiResponse.Data;
                    }
                }
            }

            return View(examReports);
        }


    }
}

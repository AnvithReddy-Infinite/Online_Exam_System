using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using OnlineExaminationSystem.Models;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly string apiBase = "https://localhost:44384/";

        // =========================
        // GET: Admin Login
        // =========================
        public ActionResult Login()
        {
            return View();
        }

        // =========================
        // POST: Admin Login
        // =========================
        [HttpPost]
        public async Task<ActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                var response = await client.PostAsync(
                    $"api/admin/admin-login?email={model.Email}&password={model.Password}",
                    null
                );

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Invalid email or password";
                    return View(model);
                }

                var result = await response.Content.ReadAsStringAsync();

                if (result.Contains("Invalid"))
                {
                    ViewBag.Error = "Invalid email or password";
                    return View(model);
                }

                // Login success
                dynamic admin = JsonConvert.DeserializeObject(result);
                Session["AdminId"] = admin.AdminId;
                Session["AdminName"] = admin.FullName;

                return RedirectToAction("Index", "AdminWelcome");
            }
        }

        // =========================
        // GET: Reset Password
        // =========================
        public ActionResult ResetPassword()
        {
            return View();
        }

        // =========================
        // POST: Reset Password
        // =========================
        [HttpPost]
        public async Task<ActionResult> ResetPassword(AdminResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBase);

                var payload = new
                {
                    Email = model.Email,
                    NewPassword = model.NewPassword
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "api/admin/admin-reset-password",
                    content
                );

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Password reset failed");
                    return View(model);
                }
            }

            TempData["Success"] = "Password reset successful. Please login.";
            return RedirectToAction("Login");
        }

        
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Welcome");
        }
    }
}

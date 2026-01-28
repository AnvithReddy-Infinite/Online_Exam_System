using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace OnlineExaminationSystem.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: AdminLogin/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string apiUrl = "https://localhost:44384/api/admin/admin-login";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44384/");
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // API expects email & password as QUERY params
                var response = await client.PostAsync(
                    $"api/admin/admin-login?email={model.Email}&password={model.Password}",
                    null
                );

                var result = await response.Content.ReadAsStringAsync();

                // CASE 1: Invalid login
                if (result.Contains("Invalid email or password"))
                {
                    ViewBag.Error = "Invalid email or password";
                    return View(model);
                }

                // CASE 2: Valid login
                var admin = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);

                Session["AdminId"] = admin.AdminId;
                Session["AdminName"] = admin.FullName;
                Session["AdminEmail"] = admin.Email;

                return RedirectToAction("Index", "AdminDashboard");

            }
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Reset link sent to your email";
            }

            return View(model);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "AdminLogin");
        }




    }
}
using Newtonsoft.Json;
using OnlineExaminationSystem.Common;
using OnlineExaminationSystem.Models.User;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace OnlineExaminationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly string apiBaseUrl = "https://localhost:44384/api/user/";

        // ---------- HOME ----------
        public ActionResult Home()
        {
            // Check login status
            bool isLoggedIn = Session["UserId"] != null;

            ViewBag.IsLoggedIn = isLoggedIn;

            if (isLoggedIn)
            {
                ViewBag.FullName = Session["FullName"]?.ToString();
            }

            return View();
        }

        // ---------- LOGIN ----------

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsJsonAsync("login", model);
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to reach server.");
                    return View(model);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                ApiResponse<UserResponseDTO> apiResponse;

                try
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse<UserResponseDTO>>(responseContent);
                }
                catch
                {
                    ModelState.AddModelError("", "Invalid response from server.");
                    return View(model);
                }

                if (apiResponse == null)
                {
                    ModelState.AddModelError("", "Invalid response from server.");
                    return View(model);
                }

                if (!apiResponse.Success)
                {
                    ModelState.AddModelError("", apiResponse.Message ?? "Invalid email or password.");
                    return View(model);
                }


                Session["UserId"] = apiResponse.Data.UserId;
                Session["FullName"] = apiResponse.Data.FullName;
                Session["Email"] = apiResponse.Data.Email;

                return RedirectToAction("StartExam", "Exam");
            }
        }


        // ---------- REGISTER ----------

        public ActionResult Register()
        {
            GenerateTextCaptcha();
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserDTO model, string CaptchaInput)
        {
            if (Session["TextCaptcha"] == null ||
                CaptchaInput != Session["TextCaptcha"].ToString())
            {
                ViewBag.CaptchaError = "Invalid captcha";
                GenerateTextCaptcha();
                return View(model);
            }

            // Your registration logic here

            return RedirectToAction("Login");
        }

        public JsonResult RefreshTextCaptcha()
        {
            GenerateTextCaptcha();
            return Json(Session["TextCaptcha"], JsonRequestBehavior.AllowGet);
        }

        private void GenerateTextCaptcha()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random rnd = new Random();

            Session["TextCaptcha"] = new string(
                Enumerable.Range(0, 5)
                .Select(x => chars[rnd.Next(chars.Length)])
                .ToArray());
        }



        // ---------- RESET PASSWORD ----------

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsJsonAsync("reset-password", model);
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to reach server.");
                    return View(model);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                ApiResponse<object> apiResponse;

                try
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseContent);
                }
                catch
                {
                    ModelState.AddModelError("", "Invalid response from server.");
                    return View(model);
                }

                if (apiResponse == null)
                {
                    ModelState.AddModelError("", "Invalid response from server.");
                    return View(model);
                }

                if (!apiResponse.Success)
                {
                    ModelState.AddModelError("", apiResponse.Message ?? "Password reset failed.");
                    return View(model);
                }

                TempData["SuccessMessage"] = apiResponse.Message ?? "Password reset successfully.";
                return RedirectToAction("Login");
            }
        }


        // ---------- LOGOUT ----------

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}

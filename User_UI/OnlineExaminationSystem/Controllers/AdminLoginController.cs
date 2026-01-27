using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExaminationSystem.Models;


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
        public ActionResult Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "AdminTechnology");
            }

            return View(model);
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
            // Later: clear session
            return RedirectToAction("Login", "AdminLogin");
        }



    }
}
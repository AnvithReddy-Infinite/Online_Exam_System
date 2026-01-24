using OnlineExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }
        // LOGIN
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Instructions");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool emailExists = model.Email == "test@gmail.com"; 
            if (!emailExists)
            {
                ModelState.AddModelError("Email", "Email not found");
                return View(model);
            }
            TempData["SuccessMessage"] = "Password reset link sent to your email";
            return RedirectToAction("Login");
        }

        public ActionResult SelectExam()
        {
            return View();
        }
        public ActionResult Instructions()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Questions()
        {
            return View();
        }


        public ActionResult Report()
        {
            return View();
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Home");
        }
    }
}
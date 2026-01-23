using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Logout()
        {
            // Later: clear session
            return RedirectToAction("Login", "AdminLogin");
        }

    }
}
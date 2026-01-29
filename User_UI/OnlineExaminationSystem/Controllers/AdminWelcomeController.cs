using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminWelcomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["AdminId"] == null)
                return RedirectToAction("Login", "AdminLogin");

            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExaminationSystem.Models;


namespace OnlineExaminationSystem.Controllers
{
    public class AdminExamController : Controller
    {
        // GET: AdminExam
        public ActionResult Index()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            return View();
        }

       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExaminationSystem.Models;


namespace OnlineExaminationSystem.Controllers
{
    public class AdminQuestionController : Controller
    {
        // GET: AdminQuestion
        public ActionResult Index()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AdminQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Later: save question to DB
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
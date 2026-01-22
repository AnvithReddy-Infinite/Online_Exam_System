using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExaminationSystem.Controllers
{
    public class AdminQuestionController : Controller
    {
        // GET: AdminQuestion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}
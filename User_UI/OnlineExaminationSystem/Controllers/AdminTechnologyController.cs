using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExaminationSystem.Models;


namespace OnlineExaminationSystem.Controllers
{
    public class AdminTechnologyController : Controller
    {
        // GET: AdminTechnology
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminTechnology/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AdminTechnologyViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4.ElmahLogViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = string.Empty;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = string.Empty;

            return View();
        }
    }
}
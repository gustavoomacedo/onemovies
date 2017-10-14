using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_PIV.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (Session["Admin"] == null)
            //    return RedirectToAction("Index", "Login");

            return View();
        }
    }
}
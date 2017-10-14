using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto_PIV.Controllers
{
    public class FilmesController : Controller
    {
        // GET: Filmes
        public ActionResult Index()
        {
            return View();
        }
    }
}
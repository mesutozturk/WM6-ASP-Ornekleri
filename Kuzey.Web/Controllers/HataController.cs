using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kuzey.Web.Controllers
{
    public class HataController : Controller
    {
        // GET: Hata
        public ActionResult Index()
        {
            Response.StatusCode = 404;
            return View();
        }
        public ActionResult H500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
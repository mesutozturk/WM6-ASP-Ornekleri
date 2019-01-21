using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IlkMvcSayfam.Models;

namespace IlkMvcSayfam.Controllers
{
    public class HomeController : Controller
    {
       static List<Kisi> kisiler = new List<Kisi>()
        {
            new Kisi()
            {
                Name = "Kamil",
                Surname = "Fidil"
            },
            new Kisi()
            {
                Name = "Hakkı",
                Surname = "Fodul"
            },
            new Kisi()
            {
                Name = "Falan",
                Surname = "Filan"
            },
        };
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Falan()
        {
            var data = kisiler;

            return View(data);
        }

        public ActionResult Detail(Guid? id)
        {
            var kisi = kisiler.FirstOrDefault(x => x.Id == id);
            if (kisi == null)
                return RedirectToAction("Falan");
            return View(kisi);
        }
    }
}
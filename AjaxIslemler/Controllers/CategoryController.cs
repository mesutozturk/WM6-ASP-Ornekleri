using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AjaxIslemler.Models;

namespace AjaxIslemler.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Search(string s)
        {
            var key = s.ToLower();
            if (key.Length <= 2)
            {
                return Json(new ResponseData()
                {
                    message = "Aramak icin 2 karakterden fazlasini girin",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var db = new NorthwindEntities();
                db.Configuration.LazyLoadingEnabled = false;
                var data = db.Categories
                    .Where(x =>
                    x.CategoryName.ToLower().Contains(key)
                    || x.Description.Contains(key))
                    .ToList();
                return Json(new ResponseData()
                {
                    message = $"{data.Count} adet kayit bulundu",
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using System;
using IlkMvcSayfam.Models;
using System.Linq;
using System.Web.Mvc;

namespace IlkMvcSayfam.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var data = new NorthwindEntities()
                .Categories
                .OrderBy(x => x.CategoryName)
                .ToList();
            return View(data);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var data = new NorthwindEntities().Categories.Find(id.Value);
            if (data == null) RedirectToAction("Index");

            return View(data);
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            try
            {
                var db = new NorthwindEntities();
                db.Categories.Add(category);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            var db = new NorthwindEntities();
            try
            {
                var category = db.Categories.Find(id.GetValueOrDefault());
                if (category == null) return RedirectToAction("Index");

                db.Categories.Remove(category);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }

        public JsonResult Categories()
        {
            var categoriler = new NorthwindEntities().Categories.Select(x => new
            {
                x.CategoryName,
                x.CategoryID,
                x.Description,
                ProductCount = x.Products.Count
            }).ToList();

            return Json(categoriler, JsonRequestBehavior.AllowGet);
        }
    }
}
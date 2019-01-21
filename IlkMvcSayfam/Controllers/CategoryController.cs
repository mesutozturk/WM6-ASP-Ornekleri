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
    }
}
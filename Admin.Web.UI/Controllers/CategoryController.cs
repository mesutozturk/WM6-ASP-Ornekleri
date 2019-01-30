using System.Collections.Generic;
using Admin.BLL.Repository;
using System.Linq;
using System.Web.Mvc;
using Admin.Models.Entities;

namespace Admin.Web.UI.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();

            return View();
        }
    }
}
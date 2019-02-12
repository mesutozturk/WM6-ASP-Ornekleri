using AutoMapper;
using Kuzey.BLL.Repository;
using Kuzey.Models.Entities;
using Kuzey.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Kuzey.Web.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var data = new CategoryRepo().GetAll().Select(x => Mapper.Map<CategoryViewModel>(x)).ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult Add(CategoryViewModel model)
        {
            new CategoryRepo().Insert(Mapper.Map<CategoryViewModel, Category>(model));
            return View();
        }
    }
}
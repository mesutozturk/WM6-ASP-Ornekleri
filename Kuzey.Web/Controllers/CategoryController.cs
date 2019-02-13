using AutoMapper;
using Kuzey.BLL.Repository;
using Kuzey.Models.Entities;
using Kuzey.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;
using Kuzey.Web.App_Code;

namespace Kuzey.Web.Controllers
{
    [ExceptionHandlerFilter]
    [RoutePrefix("Kategori")]
    public class CategoryController : Controller
    {
        // GET: Category
        [Route]
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

        [HttpGet]
        [Route("~/Detay/{kategoriadi}/{id?}")]
        public ActionResult Detail(int id = 0)
        {
            var model = new CategoryRepo().GetById(id);
            var data = Mapper.Map<CategoryViewModel>(model);
            return View(data);
        }
    }
}
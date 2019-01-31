using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.ProductList = GetProductSelectList();
            ViewBag.CategoryList = GetCategorySelectList();
            return View();
        }
    }
}
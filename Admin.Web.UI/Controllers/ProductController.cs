using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Web.Mvc;
using Admin.BLL.Helpers;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Models.ViewModels;

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Add(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductList = GetProductSelectList();
                ViewBag.CategoryList = GetCategorySelectList();
                return View(model);
            }

            try
            {
                if (model.SupProductId.ToString().Replace("0", "").Replace("-", "").Length == 0)
                    model.SupProductId = null;

                model.LastPriceUpdateDate = DateTime.Now;
                await new ProductRepo().InsertAsync(model);
                TempData["Message"] = $"{model.ProductName} isimli ürün başarıyla eklenmiştir";
                return RedirectToAction("Add");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
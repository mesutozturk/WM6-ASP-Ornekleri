using System;
using System.Collections.Generic;
using Admin.BLL.Repository;
using System.Linq;
using System.Web.Mvc;
using Admin.Models.Entities;
using Admin.Models.ViewModels;

namespace Admin.Web.UI.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            try
            {
                if (model.SupCategoryId == 0) model.SupCategoryId = null;
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("CategoryName", "100 karakteri geçme kardeş");
                    model.SupCategoryId = model.SupCategoryId ?? 0;
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }
                new CategoryRepo().Insert(model);
                ViewBag.Message = $"{model.CategoryName} isimli kategori başarıyla eklenmiştir";
                return RedirectToAction("Add");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category"
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using Admin.BLL.Repository;
using System.Linq;
using System.Web.Mvc;
using Admin.BLL.Helpers;
using Admin.Models.Entities;
using Admin.Models.Enums;
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
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

                if (model.SupCategoryId > 0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryId).TaxRate;
                }
                new CategoryRepo().Insert(model);
                TempData["Message"] = $"{model.CategoryName} isimli kategori başarıyla eklenmiştir";
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id = 0)
        {
            ViewBag.CategoryList = GetCategorySelectList();
            var data = new CategoryRepo().GetById(id);
            if (data == null)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Kategori Bulunamadı",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");
            }

            return View(data);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(Category model)
        {
            try
            {
                if (model.SupCategoryId == 0) model.SupCategoryId = null;
                if (!ModelState.IsValid)
                {
                    model.SupCategoryId = model.SupCategoryId ?? 0;
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }

                if (model.SupCategoryId > 0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryId).TaxRate;
                }
                var data = new CategoryRepo().GetById(model.Id);
                data.CategoryName = model.CategoryName;
                data.TaxRate = model.TaxRate;
                data.SupCategoryId = model.SupCategoryId;
                new CategoryRepo().Update(data);
                foreach (var dataCategory in data.Categories)
                {
                    dataCategory.TaxRate = data.TaxRate;
                    new CategoryRepo().Update(dataCategory);
                    if(dataCategory.Categories.Any())
                        UpdateSubTaxRate(dataCategory.Categories);
                }

                void UpdateSubTaxRate(ICollection<Category> dataC)
                {
                    foreach (var dataCategory in dataC)
                    {
                        dataCategory.TaxRate = data.TaxRate;
                        new CategoryRepo().Update(dataCategory);
                        if (dataCategory.Categories.Any())
                            UpdateSubTaxRate(dataCategory.Categories);
                    }
                }
                TempData["Message"] = $"{model.CategoryName} isimli kategori başarıyla güncellenmiştir";
                ViewBag.CategoryList = GetCategorySelectList();
                return View(data);
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
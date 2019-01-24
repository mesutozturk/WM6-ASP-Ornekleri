using AjaxIslemler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AjaxIslemler.Models.ViewModels;

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
            if (key.Length <= 2 && key != "*")
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
                List<CategoryViewModel> data;
                if (key == "*")
                {
                    data = db.Categories.OrderBy(x => x.CategoryName)
                        .Select(x => new CategoryViewModel()
                        {
                            CategoryName = x.CategoryName,
                            Description = x.Description,
                            CategoryID = x.CategoryID,
                            ProductCount = x.Products.Count
                        }).ToList();
                }
                else
                {
                    data = db.Categories
                        .Where(x =>
                            x.CategoryName.ToLower().Contains(key)
                            || x.Description.Contains(key))
                        .Select(x => new CategoryViewModel()
                        {
                            CategoryName = x.CategoryName,
                            Description = x.Description,
                            CategoryID = x.CategoryID,
                            ProductCount = x.Products.Count
                        })
                        .ToList();
                }
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

        [HttpPost]
        public JsonResult Add(CategoryViewModel model)
        {
            try
            {
                var db = new NorthwindEntities();
                db.Categories.Add(new Category()
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description
                });
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{model.CategoryName} ismindeki kategori basariyla eklendi",
                    success = true
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

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var db = new NorthwindEntities();
                var cat = db.Categories.Find(id);
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{cat.CategoryName} ismindeki kategori basariyla silindi",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Kategori silme isleminde hata {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(Category model)
        {
            try
            {
                var db = new NorthwindEntities();
                var cat = db.Categories.Find(model.CategoryID);
                if (cat == null)
                {
                    return Json(new ResponseData()
                    {
                        message = $"Kategori bulunamadi",
                        success = false
                    }, JsonRequestBehavior.AllowGet);
                }
                cat.Description = model.Description;
                cat.CategoryName = model.CategoryName;
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    message = $"{cat.CategoryName} ismindeki kategori basariyla guncellendi",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Kategori guncelleme isleminde hata {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AjaxIslemler.Models;
using AjaxIslemler.Models.ViewModels;

namespace AjaxIslemler.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllCategories()
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Categories.Select(x => new CategoryViewModel()
                {
                    CategoryName = x.CategoryName,
                    Description = x.Description,
                    CategoryID = x.CategoryID,
                    ProductCount = x.Products.Count
                }).ToList();
                return Json(new ResponseData()
                {
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllProducts(string key)
        {
            try
            {
                var db = new NorthwindEntities();
                var query = db.Products.AsQueryable();
                if (!string.IsNullOrEmpty(key))
                {
                    key = key.ToLower();
                    query = query.Where(x => x.ProductName.ToLower().Contains(key) ||
                                       x.Category.CategoryName.ToLower().Contains(key) ||
                                       x.Supplier.CompanyName.ToLower().Contains(key));
                }
                var data = query.OrderBy(x => x.ProductName)
                    .ToList()
                    .Select(x => new ProductViewModel()
                    {
                        CategoryName = x.Category?.CategoryName,
                        AddedDate = x.AddedDate,
                        CategoryID = x.CategoryID,
                        ProductName = x.ProductName,
                        UnitsInStock = x.UnitsInStock,
                        UnitPrice = x.UnitPrice,
                        ProductID = x.ProductID,
                        AddedDateFormatted = $"{x.AddedDate:g}",
                        Discontinued = x.Discontinued,
                        QuantityPerUnit = x.QuantityPerUnit,
                        ReorderLevel = x.ReorderLevel,
                        SupplierID = x.SupplierID,
                        SupplierName = x.Supplier?.CompanyName,
                        UnitPriceFormatted = $"{x.UnitPrice:c2}",
                        UnitsOnOrder = x.UnitsOnOrder
                    })
                    .ToList();
                return Json(new ResponseData()
                {
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(Product model)
        {
            try
            {
                var db = new NorthwindEntities();
                model.CategoryID = model.CategoryID == 0 ? null : model.CategoryID;
                model.AddedDate = DateTime.Now;
                db.Products.Add(model);
                db.SaveChanges();
                return Json(new ResponseData()
                {
                    success = true,
                    message = $"{model.ProductName} isimli ürün basariyla eklenmiştir."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
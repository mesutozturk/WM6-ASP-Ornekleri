using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Kuzey.BLL.Repository;
using Kuzey.Models.Entities;
using Kuzey.Models.ViewModels;

namespace Kuzey.Web.Controllers
{
    [RoutePrefix("kampanyali-urunler")]
    public class ProductController : Controller
    {
        // GET: Product
        [Route]
        public ActionResult Index()
        {
            var data = new ProductRepo().GetAll().Select(x => Mapper.Map<ProductViewModel>(x)).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Add(ProductViewModel model)
        {
            new ProductRepo().Insert(Mapper.Map<ProductViewModel, Product>(model));
            return View();
        }

        [HttpGet]
        [Route("~/en-ucuz-urun/{kategoriadi}-{urunadi}/{id?}")]
        public ActionResult Detail(int id = 0)
        {
            var data = new ProductRepo().GetById(id);
            if (data == null)
                RedirectToAction("Index");
            var model = Mapper.Map<ProductViewModel>(data);
            return View(model);
        }
    }
}
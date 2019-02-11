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
    public class ProductController : Controller
    {
        // GET: Product
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
    }
}
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
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            var data = new EmployeeRepo()
                .GetAll()
                .OrderBy(x => x.FirstName)
                .Select(x => Mapper.Map<EmployeeViewModel>(x)).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Add(EmployeeViewModel model)
        {
            new EmployeeRepo().Insert(Mapper.Map<EmployeeViewModel, Employee>(model));
            return View();
        }
    }
}
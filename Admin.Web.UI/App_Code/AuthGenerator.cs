using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Web.UI.Controllers;

namespace Admin.Web.UI.App_Code
{
    public class AuthGenerator
    {
        public AuthGenerator()
        {
            this.Generate();
        }

        void Generate()
        {
            var baseClass = typeof(BaseController);
            Assembly asm = Assembly.GetAssembly(typeof(Admin.Web.UI.MvcApplication));

            var controllerActionList = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new AuthOperation
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            var repo = new AuthOperatonRepo();
            foreach (var item in controllerActionList)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.Attributes))
                        item.Attributes = "HttpGet";
                    if (repo.Queryable().Any(x => x.Action == item.Action && x.Controller == item.Controller && x.Attributes == item.Attributes))
                        continue;
                    repo.Insert(item);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }
}
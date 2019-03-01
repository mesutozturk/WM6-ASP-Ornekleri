using Admin.BLL.Identity;
using Admin.Models.Enums;
using Admin.Models.IdentityModels;
using Admin.Web.UI.App_Start;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Routing;

namespace Admin.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var roller = Enum.GetNames(typeof(IdentityRoles));

            var roleManager = MembershipTools.NewRoleManager();
            foreach (var rol in roller)
            {
                if (!roleManager.RoleExists(rol))
                    roleManager.Create(new Role()
                    {
                        Name = rol
                    });
            }
        }
    }
}



using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Admin.Web.UI.App_Code
{
    public class MyAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    }
}
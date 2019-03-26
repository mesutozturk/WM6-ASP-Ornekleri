using Admin.BLL.Identity;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.App_Code
{
    public class MyAuthAttribute : AuthorizeAttribute
    {
        private static List<AuthOperation> _authOperations;
        private static List<Role> _roles;
        private static readonly char[] _splitParameter = new char[1]
        {
            ','
        };
        private string[] _rolesSplit = new string[0];
        private string[] _usersSplit = new string[0];

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this._rolesSplit = MyAuthAttribute.SplitString(Roles);
            this._usersSplit = MyAuthAttribute.SplitString(Users);

            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));
            if (OutputCacheAttribute.IsChildActionCacheActive((ControllerContext)filterContext))
                throw new InvalidOperationException();
            if ((filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ? 1 : (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ? 1 : 0)) != 0)
                return;

            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(new HttpCacheValidateHandler(this.CacheValidateHandler), (object)null);
            }
            else
                this.HandleUnauthorizedRequest(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));
            IPrincipal user = httpContext.User;

            return user.Identity.IsAuthenticated && CheckDb(httpContext);



            //var result = user.Identity.IsAuthenticated &&
            //             (this._usersSplit.Length == 0 ||
            //              ((IEnumerable<string>)this._usersSplit).Contains<string>(user.Identity.Name,
            //                  (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)) &&
            //             (this._rolesSplit.Length == 0 || ((IEnumerable<string>)this._rolesSplit).Any<string>(new Func<string, bool>(user.IsInRole)));
            //return result;
        }

        protected virtual bool CheckDb(HttpContextBase httpContext)
        {
            IPrincipal user = httpContext.User;

            var requestContext = ((MvcHandler)httpContext.Handler).RequestContext;
            var controllerName = (string)requestContext.RouteData.Values["controller"];
            controllerName = controllerName.ToLower(new CultureInfo("en-US"));
            var actionName = (string)requestContext.RouteData.Values["action"];
            actionName = actionName.ToLower(new CultureInfo("en-US"));
            var method = httpContext.Request.HttpMethod;
            method = method.ToLower(new CultureInfo("en-US"));

            var authRepo = new AuthOperatonRepo();
            if (_authOperations == null || !_authOperations.Any())
            {
                _authOperations = authRepo.GetAll().OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            }

            var authList = _authOperations;
            var authOperationResult = authList.FirstOrDefault(x =>
                x.Controller.ToLower(new CultureInfo("en-US")).Contains(controllerName) &&
                x.Action.ToLower(new CultureInfo("en-US")).Contains(actionName) &&
                x.Attributes.ToLower(new CultureInfo("en-US")).Contains(method));

            if (authOperationResult == null || !authOperationResult.AuthOperationRoles.Any()) return false;

            var roleList = authOperationResult.AuthOperationRoles.Select(x => x.Id2).ToList();
            if (_roles == null || !_roles.Any())
                _roles = MembershipTools.NewRoleManager().Roles.ToList();


            var roleName = _roles.FirstOrDefault(x => roleList.Contains(x.Id))?.Name;

            return user.IsInRole(roleName);
        }
        private void CacheValidateHandler(
            HttpContext context,
            object data,
            ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization((HttpContextBase)new HttpContextWrapper(context));
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
                return new string[0];
            return ((IEnumerable<string>)original.Split(MyAuthAttribute._splitParameter)).Select(piece => new
            {
                piece = piece,
                trimmed = piece.Trim()
            }).Where(param1 => !string.IsNullOrEmpty(param1.trimmed)).Select(param1 => param1.trimmed).ToArray<string>();
        }
    }
}
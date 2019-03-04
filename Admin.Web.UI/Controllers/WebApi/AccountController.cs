using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Admin.BLL.Identity;
using Admin.Models.Models;
using Admin.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace Admin.Web.UI.Controllers.WebApi
{
    public class AccountController : ApiController
    {
        [Authorize]
        public IHttpActionResult GetLoginInfo()
        {
            var userManager = MembershipTools.NewUserManager();
            var user = userManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            return Ok(new ResponseData()
            {
                data = new UserProfileViewModel()
                {
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    AvatarPath = user.AvatarPath,
                    Surname = user.Surname,
                    Id = user.Id
                },
                success = true
            });
        }
    }
}

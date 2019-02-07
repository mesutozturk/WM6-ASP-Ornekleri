using System;
using System.Threading.Tasks;
using System.Web;
using Admin.BLL.Helpers;
using Admin.BLL.Identity;
using Admin.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(Admin.Web.UI.App_Start.Startup))]

namespace Admin.Web.UI.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            ConfigureGoogle(app);
        }

        private void ConfigureGoogle(IAppBuilder app)
        {
            const string clientId = "648995939983-jhm6d50u9m7s7ls9ek0vbqt6nr60e2bu.apps.googleusercontent.com";
            const string clientSecret = "Pba8xj8P1SwgYfiuBxScY514";

            var options = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = async context =>
                    {
                        ExternalLoginInfo loginInfo = HttpContext.Current.GetOwinContext().Authentication.GetExternalLoginInfo();
                        // Retrieve the OAuth access token to store for subsequent API calls
                        string accessToken = context.AccessToken;

                        // Retrieve the name of the user in Google
                        string googleName = context.Name;

                        // Retrieve the user's email address
                        string googleEmailAddress = context.Email;

                        // You can even retrieve the full JSON-serialized user

                        var userDetail = context.User;

                        string id = (dynamic)context.Id;

                        string email = (dynamic)context.Email;
                        var userStore = MembershipTools.NewUserStore();
                        var userManager = new UserManager<User>(userStore);
                        var currentUser = userManager.FindByEmail(email);
                        if (currentUser == null)
                        {
                            var serializedUser = context.User;
                            try
                            {
                                var user = new User()
                                {
                                    Email = email,
                                    Name = context.GivenName,
                                    Surname = context.FamilyName,
                                    UserName = email.Substring(0, email.IndexOf('@')),
                                };
                                await userManager.CreateAsync(user, StringHelpers.GetCode().Substring(0, 6));
                                userManager.AddToRole(user.Id, "User");
                                //currentUser.EmailConfirmed = true;
                                //currentUser.Name = context.GivenName;
                                //currentUser.Surname = context.FamilyName;
                                //currentUser.RegisterDate = DateTime.Now;
                                var avatar = userDetail.SelectToken("image").SelectToken("url").ToString();
                                avatar = avatar.Substring(0, avatar.IndexOf('?'));
                                //currentUser.AvatarPath = avatar;


                                var result = userManager.AddLoginAsync(user.Id, loginInfo.Login);
                            }
                            catch (Exception ex)
                            {
                                string x = ex.StackTrace.ToString();
                            }
                        }
                        else
                        {
                            var result = userManager.AddLogin(currentUser.Id, loginInfo.Login);
                        }
                    }
                }
            };
            app.UseGoogleAuthentication(options);
        }
    }
}

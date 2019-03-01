using Admin.BLL.Helpers;
using Admin.BLL.Identity;
using Admin.Models.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Web;
using Admin.Web.UI.Controllers.WebApi.Auth;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;

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

            ConfigureOAuth(app);
            app.UseCors(CorsOptions.AllowAll);

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            ConfigureGoogle(app);
            ConfigureFacebook(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(9999),
                AllowInsecureHttp = true,
                Provider = new Provider()
            };

            app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureFacebook(IAppBuilder app)
        {
            const string appId = "345283286312425";
            const string appSecret = "447e03a0096467eb0b60483c415168d4";
            var options = new FacebookAuthenticationOptions
            {
                AppId = appId,
                AppSecret = appSecret
            };
            options.Scope.Add("email");
            options.Provider = new FacebookAuthenticationProvider()
            { 
                OnAuthenticated = async context =>
                {
                    var userDetail = context.User;

                    string id = (dynamic)context.Id;

                    string email = (dynamic)context.Email;
                    var userStore = MembershipTools.NewUserStore();
                    var userManager = new UserManager<User>(userStore);
                    var currentUser = userManager.FindByEmail(email);
                    if (currentUser != null)
                    {
                        var serializedUser = context.User;
                        try
                        {
                            currentUser.EmailConfirmed = true;
                            currentUser.Name = ((dynamic)context.User).first_name.Value;
                            currentUser.Surname = ((dynamic)context.User).last_name.Value;
                            //currentUser.RegisterDate = DateTime.Now;
                            var avatar = @"https://graph.facebook.com/" + id + "/picture?type=large";

                            var loginInfo = HttpContext.Current.GetOwinContext().Authentication.GetExternalLoginInfo();
                            if (loginInfo != null)
                            {
                                var result = userManager.AddLogin(currentUser.Id, loginInfo.Login);
                                Console.WriteLine("test");
                            }
                            await userStore.UpdateAsync(currentUser);
                            await userStore.Context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            string x = ex.StackTrace.ToString();
                        }
                    }
                    else
                    {
                        var user = new User()
                        {
                            Email = email,
                            Name = ((dynamic) context.User).first_name.Value,
                            Surname = ((dynamic) context.User).last_name.Value,
                            UserName = email.Substring(0, email.IndexOf('@'))
                        };
                        userManager.Create( user,StringHelpers.GetCode().Substring(0, 6));
                        var loginInfo = HttpContext.Current.GetOwinContext().Authentication.GetExternalLoginInfo();
                        var result = userManager.AddLogin(user.Id, loginInfo.Login);
                    }
                }
            };
            app.UseFacebookAuthentication(options);
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

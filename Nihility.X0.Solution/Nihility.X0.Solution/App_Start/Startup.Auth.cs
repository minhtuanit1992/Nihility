using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Nihility.X0.Solution.Providers;
using Nihility.X0.Solution.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Facebook;
using System.Configuration;
using Nihility.Data;
using Nihility.Model.Models.Identity;

namespace Nihility.X0.Solution
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Cấu hình đăng kí để sử dụng đối tượng DB context để lưu trữ trong OwinContext 
            app.CreatePerOwinContext(NihiDbContext.Create);
            // Đối tượng UserManager để sử dụng cho mỗi lần Request
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            // Cho phép ứng dụng sử dụng cookie để lưu trữ thông tin cho người dùng đã đăng nhập          
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                Provider = new CookieAuthenticationProvider
                {
                    // Cho phép ứng dụng xác thực bảo mật khi người dùng đăng nhập
                    // Đây là một tính năng bảo mật được sử dụng khi thay đổi mật khẩu hoặc thêm thông tin đăng nhập của bên thứ 3 vào tài khoản.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Sử dụng cookie để tạm thời lưu trữ thông tin về người dùng đăng nhập bằng nhà cung cấp đăng nhập của bên thứ ba như : Facebook, Google, Zalo
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            // Sử dụng cookie để lưu trữ thông tin xác thực dịch vụ 2FA
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            // Cấu hình ứng dụng cho lược đồ OAuth
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // Cài đặt chế độ không an toàn cho HTTP: AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Cho phep ứng dụng sử dụng Bearer Tokens để xác thực người dùng, Ví dụ: JWT
            app.UseOAuthBearerTokens(OAuthOptions);

            // Bỏ Comment tương ứng để cho phép đăng nhập với hình thức đăng nhập của bên thứ 3
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            FacebookAuthenticationOptions facebookOptions = new FacebookAuthenticationOptions()
            {
                AuthenticationType = "FaceBook",
                AppId = ConfigurationManager.AppSettings["FBClientID"].ToString(),
                AppSecret = ConfigurationManager.AppSettings["FBClientSecret"].ToString(),
                //The class you have defined beforehand
                //BackchannelHttpHandler = new FacebookBackChannelHandler(),
                //You can difine facebook version yourdself(VX.X below)
                //UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email"
            };
            app.UseFacebookAuthentication(facebookOptions);

            GoogleOAuth2AuthenticationOptions googleOptions = new GoogleOAuth2AuthenticationOptions()
            {
                AuthenticationType = "Google",
                ClientId = ConfigurationManager.AppSettings["GClientID"].ToString(),
                ClientSecret = ConfigurationManager.AppSettings["GClientSecret"].ToString()
            };
            app.UseGoogleAuthentication(googleOptions);
        }
    }
}

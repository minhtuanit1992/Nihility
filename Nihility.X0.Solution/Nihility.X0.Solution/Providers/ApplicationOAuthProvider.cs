using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Nihility.Model.Models.Identity;
using Nihility.X0.Solution.Models;

namespace Nihility.X0.Solution.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider()
        {

        }

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                context.SetError("server_error", "Lỗi trong quá trình xử lý.");
                context.Rejected();
                return;
            }

            if (user == null)
            {
                context.SetError("invalid_grant", "Tên tài khoản hoặc mật khẩu không chính xác");
                return;
            }
            else if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "Tài khoản đang chờ được phê duyệt");
                return;
            }
            else
            {
                //var permissions = ServiceFactory.Get<IPermissionService>().GetByUserId(user.Id);
                //var permissionViewModels = AutoMapper.Mapper.Map<ICollection<Permission>, ICollection<PermissionViewModel>>(permissions);
                //var roles = userManager.GetRoles(user.Id);
                //ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                //string avatar = string.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar;
                //string email = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                //identity.AddClaim(new Claim("fullName", user.FullName));
                //identity.AddClaim(new Claim("avatar", avatar));
                //identity.AddClaim(new Claim("email", email));
                //identity.AddClaim(new Claim("username", user.UserName));
                //identity.AddClaim(new Claim("roles", JsonConvert.SerializeObject(roles)));
                //identity.AddClaim(new Claim("permissions", JsonConvert.SerializeObject(permissionViewModels)));
                //var props = new AuthenticationProperties(new Dictionary<string, string>
                //    {
                //        {"fullName", user.FullName},
                //        {"avatar", avatar },
                //        {"email", email},
                //        {"username", user.UserName},
                //        {"permissions",JsonConvert.SerializeObject(permissionViewModels) },
                //        {"roles",JsonConvert.SerializeObject(roles) }

                //    });
                //context.Validated(new AuthenticationTicket(identity, props));
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
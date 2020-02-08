using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Model.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(256)]
        public string FullName { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        public Nullable<DateTime> BirthDay { get; set; }
        public bool IsGoogleAuthenticatorEnabled { get; set; }
        public string GoogleAuthenticatorSecretKey { get; set; }
        public string TwoFactorType { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Lưu ý authenticationType phải khớp với CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Thêm cấu hình Claim tại đây
            return userIdentity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager">TUser</param>
        /// <param name="authenticationType">Authentication Type</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Lưu ý xác thực phải khớp với xác định trong CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Tùy chỉnh yêu cầu người dùng tại đây
            return userIdentity;
        }
    }
}

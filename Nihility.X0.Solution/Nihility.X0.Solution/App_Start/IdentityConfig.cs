using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Nihility.Data;
using Nihility.Model.Models.Identity;
using Nihility.X0.Solution.Models;
using Nihility.X0.Solution.Providers;

namespace Nihility.X0.Solution
{

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Viết Plugin gửi SMS ở đây
            return Task.FromResult(0);
        }
    }

    //Cấu hình cho UserManager dùng trong ứng dụng. UserManager được định nghĩa trong ASP.NET Identity và được sử dụng bởi ứng dụng
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
            // Khởi tạo dịch vụ gửi email(Không có thì gửi bằng niềm)
            //EmailService = new EmailService();
            SmsService = new SmsService();
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<NihiDbContext>()));
            // Cấu hình xác thực cho tên người dùng
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                // Chỉ cho phép tên là chữ và số
                AllowOnlyAlphanumericUserNames = false,
                // Bắt buộc UserName phải là Email
                RequireUniqueEmail = true,
            };
            // Cấu hình xác thực cho mật khẩu
            manager.PasswordValidator = new PasswordValidator
            {
                // Độ dài bắt buộc
                RequiredLength = 6,
                // Bắt buộc phải có chữ cái và số
                RequireNonLetterOrDigit = false,
                // Bắt buộc phải có số
                RequireDigit = true,
                // Bắt buộc phải có chữ thường
                RequireLowercase = true,
                // Bắt buộc phải có chữ hoa
                RequireUppercase = true,
            };

            // Cấu hình mặc định khóa người dùng
            manager.UserLockoutEnabledByDefault = true;
            // Cấu hình mặc đình thời gian khóa
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            // Khóa sau khi thất bại n lần
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Đăng ký gói dịch vụ xác thực qua Email
            var email2FAProvider = new EmailTokenProvider<ApplicationUser>();
            email2FAProvider.Subject = "Security Code";
            email2FAProvider.BodyFormat = "Your security code is: {0}";
            manager.RegisterTwoFactorProvider("EmailCode", email2FAProvider);

            // Đăng ký gói dịch vụ xác thực qua SMS
            var sms2FAProvider = new PhoneNumberTokenProvider<ApplicationUser>();
            sms2FAProvider.MessageFormat = "Your security code is: {0}";
            manager.RegisterTwoFactorProvider("PhoneCode", sms2FAProvider);

            // Đăng ký gói dịch vụ xác thực của Google Authendicator do Admin dựng.
            var googleAuthendicator2FAProvider = new GoogleAuthenticatorTokenProvider();
            manager.RegisterTwoFactorProvider("GoogleAuthenticator", googleAuthendicator2FAProvider);

            var dataProtectionProvider = options.DataProtectionProvider;
            var userTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            if (dataProtectionProvider != null)
            {
                // Cài đặt Token cho ForgotPassword và Confirm Email là 1 giờ kể từ lúc gửi.
                userTokenProvider.TokenLifespan = TimeSpan.FromHours(1);
                manager.UserTokenProvider = userTokenProvider;
            }
            return manager;
        }
    }

    // Cấu hình ứng dụng quản lý đăng nhập
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {

        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    // Cấu hình ứng dụng quản lý phân quyền của User
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {

        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            ApplicationRoleManager applicationRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<NihiDbContext>()));

            return applicationRoleManager;
        }
    }

    //public class ApplicationUserStore : UserStore<ApplicationUser>
    //{
    //    public ApplicationUserStore(NihiDbContext context) : base(context)
    //    {

    //    }
    //}
}

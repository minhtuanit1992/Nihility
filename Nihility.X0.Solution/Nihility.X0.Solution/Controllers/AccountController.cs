using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Nihility.Model.Models.Identity;
using Nihility.X0.Solution.Models;
using Nihility.X0.Solution.Models.Account.ViewModels;
using Nihility.X0.Solution.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Nihility.X0.Solution.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Account/Login      
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// <para>Author: Hứa Minh Tuấn</para>
        /// <para>Create Date: 2/10/2020</para>
        /// </summary>
        /// <param name="model">Thông tin đăng nhập của người dùng</param>
        /// <param name="returnUrl">Nếu đăng nhập thành công sẽ trở về đường dẫn này</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                ApplicationUser currentUser = await UserManager.FindByEmailAsync(model.Email);

                if (currentUser != null)
                {
                    model.UserName = currentUser.UserName;
                }

                // Hiện tại vẫn chưa thiết lập đếm số lần đăng nhập thất bại để khóa Account. Để thiết lập khóa tài khoản khi đăng nhập thất bại quá nhiều, đổi shouldLockout là true
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            if (!await UserManager.IsEmailConfirmedAsync(currentUser.Id))
                            {
                                ModelState.AddModelError("", "Tài khoản đang chờ được phê duyệt");
                                return View(model);
                            }
                            else
                            {
                                return RedirectToLocal(returnUrl);
                            }
                        }
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Đăng nhập thất bại, vui lòng kiểm tra lại tài khoản hoặc mật khẩu");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <para>Author: Hứa Minh Tuấn</para>
        /// <para>Create Date: 2/10/2020</para>
        /// <para>Cung cấp thông tin đăng nhập mà người dùng sử dụng để đăng nhập ví dụ: Google, Facebook v.v...</para>
        /// <para>Bởi vì người dùng không thể đăng nhập với nhiều nhà cung cấp cùng một lúc</para>
        /// </summary>
        /// <param name="provider">Thông tin nhà cung cấp đăng nhập (Google, Facebook...)</param>
        /// <param name="returnUrl">Nếu quá trình đăng nhập thành công sẽ trả về đường dẫn này</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            //Thông tin chuyễn đến đến phương thức đăng nhập của bên thứ 3
            ChallengeResult result = new ChallengeResult
            {
                LoginProvider = provider,
                RedirectUri = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl })
            };

            return result;
        }

        /// <summary>
        /// <para>Author: Hứa Minh Tuấn</para>
        /// <para>Create Date: 2/10/2020</para>
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            // Kiểm tra nếu người dùng đã đăng nhập từ trước thì chuyễn người dùng đến trở về Trang chủ, tránh trường hợp đăng nhập sau khi đã đăng nhập.
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Chủ động đăng nhập với bên thứ 3 tương ứng nếu người dùng đã đăng nhập từ trước
            SignInStatus result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            if (result == SignInStatus.Failure)
            {
                ApplicationUser user = UserManager.FindByEmail(loginInfo.Email);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    user = new ApplicationUser { UserName = loginInfo.DefaultUserName, Email = loginInfo.Email };
                    IdentityResult userResult = await UserManager.CreateAsync(user);
                    if (userResult.Succeeded)
                    {
                        userResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                        if (userResult.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(userResult);
                }
            }

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Nếu người dùng vẫn chưa có tài khoản, yêu cầu người dùng tạo một tài khoản
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    ExternalLoginConfirmationViewModel model = new ExternalLoginConfirmationViewModel
                    {
                        Email = loginInfo.Email,
                        UserName = loginInfo.DefaultUserName
                    };
                    return View("ExternalLoginConfirmation", model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginConfirmation()
        {
            return View();
        }

        // POST: /Account/ExternalLoginConfirmation
        /// <summary>
        /// <para>Author: Hứa Minh Tuấn</para>
        /// <para>Create Date: 2/10/2020</para>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Lấy thông tin về người dùng từ thông tin đăng nhập cả bên thứ 3
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOff
        /// <summary>
        /// <para>Author: Hứa Minh Tuấn</para>
        /// <para>Create Date: 2/10/2020</para>
        /// <para>Đăng xuất tài khoản</para>
        /// </summary>
        /// <returns>Clear Cookie và các trạng thái đăng nhập</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            string authenticationTypes = DefaultAuthenticationTypes.ApplicationCookie;
            AuthenticationManager.SignOut(authenticationTypes);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
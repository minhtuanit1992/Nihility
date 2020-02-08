using Base32;
using Microsoft.AspNet.Identity;
using Nihility.Model.Models.Identity;
using Nihility.X0.Solution.Models;
using OtpSharp;
using System.Threading.Tasks;

namespace Nihility.X0.Solution.Providers
{
    /// <summary>
    /// Lưu ý: Để sử dụng Base32Coder, VerificationWindow, Totp, thì cần thêm "OtpShard" Package
    /// </summary>
    public class GoogleAuthenticatorTokenProvider : IUserTokenProvider<ApplicationUser, string>
    {
        /// <summary>
        /// Author: Hứa Minh Tuấn
        /// Description: Do hàm tự dựng nên không thể sinh mã, GenerateAsync tự trả về null
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult((string)null);
        }

        /// <summary>
        /// Author: Hứa Minh Tuấn
        /// Description: Xác thực mã do người dùng nhập vào
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="token"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> ValidateAsync(string purpose, string totpToken, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            int previous = 2;
            int future = 2;

            byte[] secretKey = Base32Encoder.Decode(user.GoogleAuthenticatorSecretKey);
            var otp = new Totp(secretKey);
            // Kiểm tra xem mã TOTP được người dùng nhập có hợp lệ hay không
            VerificationWindow verificationWindow = new VerificationWindow(previous, future);
            bool valid = otp.VerifyTotp(totpToken, out long timeStepMatched, verificationWindow);

            return Task.FromResult(valid);
        }

        /// <summary>
        /// Author: Hứa Minh Tuấn
        /// Description: Do hiện tại không cần gửi thông báo cho người dùng nên chỉ trả về bool là true
        /// </summary>
        /// <param name="token"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task NotifyAsync(string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Author: Hứa Minh Tuấn
        /// Description: Cho biết User có thể sử dụng Google Authendicator làm xác minh 2FA hay không
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> IsValidProviderForUserAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult(user.IsGoogleAuthenticatorEnabled);
        }
    }
}
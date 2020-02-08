using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Models.Account.ViewModels
{
    /// <summary>
    /// Author: Hứa Minh Tuấn
    /// - Lớp nhận dữ từ người dùng để truyền dữ liệu đăng nhập vào hệ thống Identity
    /// </summary>
    public class LoginViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
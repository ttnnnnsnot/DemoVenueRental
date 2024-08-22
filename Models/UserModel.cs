using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DemoVenueRental.Models
{
    public class Login
    {
        [Required(ErrorMessage = "請輸入Email")]
        [EmailAddress(ErrorMessage = "請輸入正確的格式name@example.com")]
        [MaxLength(100, ErrorMessage = "最多輸入100字")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "最少輸入6碼")]
        [MaxLength(20, ErrorMessage = "最多輸入20字")]
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class Register
    {
        [Remote(action: "CheckEmail", controller: "User", areaName: "api", ErrorMessage = "已經被使用過")]
        [Required(ErrorMessage = "請輸入Email")]
        [EmailAddress(ErrorMessage = "請輸入正確的格式name@example.com")]
        [MaxLength(100, ErrorMessage = "最多輸入100字")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "最少輸入6碼")]
        [MaxLength(20, ErrorMessage = "最多輸入20字")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入密碼")]
        [Compare("PasswordHash", ErrorMessage = "確認密碼與密碼不匹配")]
        public string ConfirmPasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入姓")]
        [MaxLength(20, ErrorMessage = "最多輸入20字")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入姓名")]
        [MaxLength(20, ErrorMessage = "最多輸入20字")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "請輸入電話")]
        [MaxLength(10, ErrorMessage = "最多輸入10字")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "請輸入正確的手機號碼格式")]
        public string Phone { get; set; } = string.Empty;
    }

    public class Users
    {
        public int UserId { get; set; }
    }
}

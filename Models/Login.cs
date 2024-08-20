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
}

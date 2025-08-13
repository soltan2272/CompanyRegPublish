using System.ComponentModel.DataAnnotations;

namespace CompanyServiceLayer.Dtos
{
    public class SetPasswordAfterOtpDto
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;
        
        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

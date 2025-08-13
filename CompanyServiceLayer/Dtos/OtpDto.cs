using System.ComponentModel.DataAnnotations;

namespace CompanyServiceLayer.Dtos
{
    public class GenerateOtpDto
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = null!;
    }

    public class VerifyOtpDto
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "OTP code is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP code must be exactly 6 characters.")]
        public string Code { get; set; } = null!;
    }

    public class OtpResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? OtpCode { get; set; } 
    }
}

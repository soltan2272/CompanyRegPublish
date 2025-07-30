using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CompanyApi.Dtos
{
    public class CompanyRegisterDTO
    {
        [Required(ErrorMessage = "Arabic name is required.")]
        public string ArabicName { get; set; } = null!;
        [Required(ErrorMessage = "English name is required.")]
        public string EnglishName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = null!;
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? WebsiteUrl { get; set; }
        public IFormFile? LogoPath { get; set; }
       
       
    }
}

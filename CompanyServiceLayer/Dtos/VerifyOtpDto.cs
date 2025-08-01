using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyServiceLayer.Dtos
{
    public class VerifyOtpDto
    {
        [Required(ErrorMessage = "Email is required."),EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "OTP is required.")]
        public string Otp { get; set; } = null!;
    }

}

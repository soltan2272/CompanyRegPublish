using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyApi.Dtos;
using CompanyServiceLayer.Dtos;

namespace CompanyServiceLayer.Interfaces
{
    public interface IAuthService
    {
        Task RegisterCompany(CompanyRegisterDTO registerDTO);
        bool VerifyOtp(VerifyOtpDto verifyOtp);
        Task<bool> SetPasswordAsync(SetPasswordDto setPasswordDto);
        Task<LoginResult> LoginAsync(LoginDto dto);


    }
}

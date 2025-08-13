using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyServiceLayer.Dtos;

namespace CompanyServiceLayer.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterCompanyAsync(CompanyRegisterDTO registerDTO);
        Task<AuthResult> RegisterEmployeeAsync(EmployeeRegisterDTO registerDTO);
        Task<AuthResult> GenerateOtpAsync(GenerateOtpDto dto);
        Task<AuthResult> VerifyOtpAsync(VerifyOtpDto dto);
        Task<AuthResult> LoginAsync(LoginDto dto);
    }
}

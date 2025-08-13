using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CompanyDataLayer;
using CompanyDataLayer.Models;
using CompanyServiceLayer.Dtos;
using CompanyServiceLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyServiceLayer.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IEmailService emailService,
            IFileService fileService,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _emailService = emailService;
            _fileService = fileService;
            _context = context;
        }

        public async Task<AuthResult> RegisterCompanyAsync(CompanyRegisterDTO registerDTO)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return new AuthResult { Success = false, Message = "Email already exists" };
                }

                var companyUser = new ApplicationUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    ArabicName = registerDTO.ArabicName,
                    EnglishName = registerDTO.EnglishName,
                    WebsiteUrl = registerDTO.WebsiteUrl,
                    IsEmailVerified = false,
                    CreatedAt = DateTime.UtcNow
                };

                if (registerDTO.LogoPath != null && registerDTO.LogoPath.Length > 0)
                {
                    var logoPath = await _fileService.
                        SaveImageAsync(registerDTO.LogoPath, "company-logos");
                    companyUser.LogoPath = logoPath;
                }

                var result = await _userManager.
                    CreateAsync(companyUser, registerDTO.Password);
                if (!result.Succeeded)
                {
                    return new AuthResult { Success = false, 
                        Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
                }

                await _userManager.AddToRoleAsync(companyUser, "Company");

                var otpCode = await GenerateAndStoreOtpAsync(companyUser.Id);

                await _emailService.SendEmailAsync(
                    registerDTO.Email,
                    "Verify your company email",
                    $"Your verification code is: {otpCode}"
                );

                return new AuthResult { Success = true, Message = "Company registered successfully. Please check your email for verification." };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Registration failed: {ex.Message}" };
            }
        }

        public async Task<AuthResult> RegisterEmployeeAsync(EmployeeRegisterDTO registerDTO)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return new AuthResult { Success = false, Message = "Email already exists" };
                }

                var company = await _userManager.FindByIdAsync(registerDTO.CompanyId);
                if (company == null || !await _userManager.IsInRoleAsync(company, "Company"))
                {
                    return new AuthResult { Success = false, Message = "Invalid company ID" };
                }

                var employeeUser = new ApplicationUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    CompanyId = registerDTO.CompanyId,
                    EmployeeId = registerDTO.EmployeeId,
                    Department = registerDTO.Department,
                    Position = registerDTO.Position,
                    HireDate = DateTime.UtcNow,
                    IsEmailVerified = false,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(employeeUser, registerDTO.Password);
                if (!result.Succeeded)
                {
                    return new AuthResult { Success = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
                }

                if (registerDTO.Role == "Manager" || registerDTO.Role == "Employee")
                {
                    await _userManager.AddToRoleAsync(employeeUser, registerDTO.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(employeeUser, "Employee");
                }

                var otpCode = await GenerateAndStoreOtpAsync(employeeUser.Id);

                await _emailService.SendEmailAsync(
                    registerDTO.Email,
                    "Verify your employee email",
                    $"Your verification code is: {otpCode}"
                );

                return new AuthResult { Success = true, Message = "Employee registered successfully. Please check your email for verification." };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Registration failed: {ex.Message}" };
            }
        }

        public async Task<AuthResult> GenerateOtpAsync(GenerateOtpDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return new AuthResult { Success = false, Message = "User not found" };
                }

                var otpCode = await GenerateAndStoreOtpAsync(user.Id);

                await _emailService.SendEmailAsync(
                    dto.Email,
                    "Your verification code",
                    $"Your verification code is: {otpCode}"
                );

                return new AuthResult { Success = true, Message = "OTP sent successfully" };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Failed to generate OTP: {ex.Message}" };
            }
        }

        public async Task<AuthResult> VerifyOtpAsync(VerifyOtpDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return new AuthResult { Success = false, Message = "User not found" };
                }

                var otpRecord = await _context.OtpCodes
                    .Where(o => o.UserId == user.Id && o.Code == dto.Code 
                    && !o.IsUsed && o.ExpiryTime > DateTime.UtcNow)
                    .FirstOrDefaultAsync();

                if (otpRecord == null)
                {
                    return new AuthResult { Success = false, Message = "Invalid or expired OTP code" };
                }

                otpRecord.IsUsed = true;
                otpRecord.UsedAt = DateTime.UtcNow;

                user.IsEmailVerified = true;
                user.VerifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new AuthResult { Success = true, Message = "Email verified successfully" };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Verification failed: {ex.Message}" };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return new AuthResult { Success = false, Message = 
                        "Invalid email or password" };
                }

                if (!user.IsEmailVerified)
                {
                    return new AuthResult { Success = false, Message = 
                        "Please verify your email first" };
                }

                var isValidPassword = await _userManager.
                    CheckPasswordAsync(user, dto.Password);
                if (!isValidPassword)
                {
                    return new AuthResult 
                    { Success = false, Message = "Invalid email or password" };
                }

                var roles = await _userManager.GetRolesAsync(user);
                
                ApplicationUser? company = null;
                if (!string.IsNullOrEmpty(user.CompanyId))
                {
                    company = await _userManager.FindByIdAsync(user.CompanyId);
                }

                return new AuthResult 
                { 
                    Success = true, 
                    Message = "Login successful",
                    UserId = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    IsEmailVerified = user.IsEmailVerified,
                    CompanyName = user.EnglishName ?? user.ArabicName,
                    CompanyArabicName = user.ArabicName,
                    CompanyEnglishName = user.EnglishName,
                    CompanyWebsite = user.WebsiteUrl,
                    CompanyLogoUrl = !string.IsNullOrEmpty(user.LogoPath) ? 
                    _fileService.GetImageUrl(user.LogoPath) : null,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmployeeId = user.EmployeeId,
                    Department = user.Department,
                    Position = user.Position,
                    HireDate = user.HireDate,

                    CompanyId = user.CompanyId
                   
                };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Login failed: {ex.Message}" };
            }
        }

        private async Task<string> GenerateAndStoreOtpAsync(string userId)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpRecord = new OtpCodes
            {
                UserId = userId,
                Code = otpCode,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.OtpCodes.Add(otpRecord);
            await _context.SaveChangesAsync();

            return otpCode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CompanyApi.Dtos;
using CompanyDataLayer;
using CompanyDataLayer.Models;
using CompanyRepositoryLayer.Interfaces;
using CompanyServiceLayer.Dtos;
using CompanyServiceLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace CompanyServiceLayer.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly IEmailService emailService;
        private readonly AppDbContext context;
        private readonly IPasswordHasher<Company> passwordHasher;

        public AuthService(ICompanyRepository companyRepository,
            IMapper mapper,
            IMemoryCache cache,
            IEmailService emailService,
            AppDbContext context,
            IPasswordHasher<Company> passwordHasher)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.cache = cache;
            this.emailService = emailService;
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        public async Task RegisterCompany(CompanyRegisterDTO registerDTO)
        {
            if (await companyRepository.EmailExistsAsync(registerDTO.Email))
            {
                throw new ArgumentException("Email already exists");
            }

            var company = mapper.Map<Company>(registerDTO);
            company.CreatedAt = DateTime.UtcNow;
            company.Id = Guid.NewGuid();
            company.IsEmailVerified = false;

            try
            {
                await companyRepository.AddAsync(company);
                await companyRepository.SaveChangesAsync();

                var otp = new Random().Next(100000, 999999).ToString();
                cache.Set(registerDTO.Email, otp, TimeSpan.FromMinutes(5));

                await emailService.SendEmailAsync(
                    registerDTO.Email,
                    "Verify your email",
                    $"Your OTP is: {otp}"
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register company. Please try again later.");
            }
        }
        public Task<bool> SetPasswordAsync(SetPasswordDto setPasswordDto)
        {
            var company = companyRepository.GetQueryable()
                .FirstOrDefault(c => c.Email == setPasswordDto.Email);
            if (company is null || !company.IsEmailVerified)
                return Task.FromResult(false);
           
            var passwordHased = passwordHasher.HashPassword(company, setPasswordDto.Password);
            company.PasswordHash = passwordHased;
            companyRepository.SaveChangesAsync();
            return Task.FromResult(true);
        }
      

        public bool VerifyOtp(VerifyOtpDto dto)
        {
            if (!cache.TryGetValue(dto.Email, out string? cachedOtp))
                return  false;

            if (cachedOtp != dto.Otp)
                return false;

            var company = companyRepository.GetQueryable().
                FirstOrDefault(c => c.Email == dto.Email);
            if (company is null)
                return false;

            company.IsEmailVerified = true;
            company.VerifiedAt = DateTime.UtcNow;
            companyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
            var company = await companyRepository.GetQueryable().
                FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (company is null || !company.IsEmailVerified)
                return null;

            var result = passwordHasher.VerifyHashedPassword(company, company.PasswordHash!, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var loginResult = new LoginResult
            {
              
                EnglishName = company.EnglishName,
                LogoPath =company.LogoPath
            };
            return loginResult;
        }


    }
}

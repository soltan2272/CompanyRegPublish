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

        public AuthService(ICompanyRepository companyRepository,IMapper mapper,IMemoryCache cache,IEmailService emailService,AppDbContext context)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.cache = cache;
            this.emailService = emailService;
            this.context = context;
        }
        public async Task RegisterCompany(CompanyRegisterDTO registerDTO)
        {
            Task<bool> ifExist = companyRepository.EmailExistsAsync(registerDTO.Email);

            if (ifExist.Result)
            {
                throw new Exception("Email already exists");
            }
            else
            {
                var company = mapper.Map<Company>(registerDTO);
                company.CreatedAt = DateTime.Now;
                company.Id = Guid.NewGuid();
                company.IsEmailVerified = false;
              await  companyRepository.AddAsync(company);
               await companyRepository.SaveChangesAsync();

                var otp = new Random().Next(100000, 999999).ToString();
                cache.Set(registerDTO.Email, otp, TimeSpan.FromMinutes(5));

                //  Send OTP via email
                emailService.SendEmailAsync(
                    registerDTO.Email,
                    "Verify your email",
                    $"Your OTP is: {otp}"
                );
                
                
            }
        }

        public bool VerifyOtp(VerifyOtpDto dto)
        {
            if (!cache.TryGetValue(dto.Email, out string? cachedOtp))
                return false;

            if (cachedOtp != dto.Otp)
                return false;

            var company = context.Companies.FirstOrDefault(c => c.Email == dto.Email);
            if (company is null)
                return false;

            company.IsEmailVerified = true;
            company.VerifiedAt = DateTime.UtcNow;
            context.SaveChanges();

            return true;
        }

    }
}

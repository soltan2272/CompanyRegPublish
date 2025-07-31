using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CompanyApi.Dtos;
using CompanyDataLayer.Models;
using CompanyRepositoryLayer.Interfaces;
using CompanyServiceLayer.Interfaces;
using Microsoft.Extensions.Caching.Memory;


namespace CompanyServiceLayer.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly IEmailService emailService;

        public AuthService(ICompanyRepository companyRepository,IMapper mapper,IMemoryCache cache,IEmailService emailService)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.cache = cache;
            this.emailService = emailService;
        }
        public void RegisterCompany(CompanyRegisterDTO registerDTO)
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
                companyRepository.AddAsync(company);
                companyRepository.SaveChangesAsync();

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
    }
}

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


namespace CompanyServiceLayer.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public AuthService(ICompanyRepository companyRepository,IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }
        public void RegisterCompany(CompanyRegisterDTO registerDTO)
        {
            Task<bool> ifExist = companyRepository.IfEmailExistsAsync(registerDTO.Email);

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
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDataLayer;
using CompanyDataLayer.Models;
using CompanyRepositoryLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompanyRepositoryLayer.Repositories
{
    public class CompanyRepository :  GenericRepository<Company> , ICompanyRepository
    {
        private readonly AppDbContext context;

        public CompanyRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }



        public async Task<bool> IfEmailExistsAsync(string email)
        {
            Company c =await context.Companies
                .FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
       
            if(c == null)
            {
                return false;
            }

            return true;
        }

       
    }
}

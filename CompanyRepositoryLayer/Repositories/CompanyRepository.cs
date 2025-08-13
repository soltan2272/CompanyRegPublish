using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDataLayer;
using CompanyDataLayer.Models;
using CompanyRepositoryLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyRepositoryLayer.Repositories
{
    public class CompanyRepository : GenericRepository<ApplicationUser>, ICompanyRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyRepository(AppDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<ApplicationUser?> GetCompanyByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsInRoleAsync(user, "Company"))
            {
                return user;
            }
            return null;
        }

        public async Task<ApplicationUser?> GetCompanyByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && await _userManager.IsInRoleAsync(user, "Company"))
            {
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<ApplicationUser>> GetEmployeesByCompanyIdAsync(string companyId)
        {
            return await _context.Users
                .Where(u => u.CompanyId == companyId && 
                           (u.CompanyId != null && u.CompanyId != ""))
                .ToListAsync();
        }
    }
}

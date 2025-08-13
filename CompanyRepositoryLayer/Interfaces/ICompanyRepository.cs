using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDataLayer.Models;

namespace CompanyRepositoryLayer.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<ApplicationUser>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<ApplicationUser?> GetCompanyByEmailAsync(string email);
        Task<ApplicationUser?> GetCompanyByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetEmployeesByCompanyIdAsync(string companyId);
    }
}

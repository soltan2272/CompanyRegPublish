using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDataLayer.Models;

namespace CompanyRepositoryLayer.Interfaces
{
    public interface ICompanyRepository :IGenericRepository<Company>
    {
        Task<bool> IfEmailExistsAsync(string email);

    }
}

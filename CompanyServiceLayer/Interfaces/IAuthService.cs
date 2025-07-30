using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyApi.Dtos;

namespace CompanyServiceLayer.Interfaces
{
    public interface IAuthService
    {
        void RegisterCompany(CompanyRegisterDTO registerDTO);

    }
}

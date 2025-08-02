using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyServiceLayer.Dtos
{
    public class SetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

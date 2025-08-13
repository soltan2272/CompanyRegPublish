using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CompanyDataLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; } 
        public bool IsEmailVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? VerifiedAt { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyId { get; set; } 
        public string? EmployeeId { get; set; } 
        public string? Department { get; set; }
        public string? Position { get; set; }
        public DateTime? HireDate { get; set; }

        public virtual ApplicationUser? Company { get; set; }
        public virtual ICollection<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
    }
}

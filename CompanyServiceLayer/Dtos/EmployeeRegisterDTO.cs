using System.ComponentModel.DataAnnotations;

namespace CompanyServiceLayer.Dtos
{
    public class EmployeeRegisterDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = null!;
        
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = null!;
        
        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; } = null!;
        
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Company ID is required.")]
        public string CompanyId { get; set; } = null!;
        
        [Required(ErrorMessage = "Employee ID is required.")]
        public string EmployeeId { get; set; } = null!;
        
        public string? Department { get; set; }
        
        public string? Position { get; set; }
        
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!; // "Employee" or "Manager"
    }
}

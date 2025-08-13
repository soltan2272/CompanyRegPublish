namespace CompanyServiceLayer.Dtos
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? Token { get; set; }

        public string? CompanyName { get; set; }
        public string? CompanyArabicName { get; set; }
        public string? CompanyEnglishName { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? CompanyLogoUrl { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeId { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public DateTime? HireDate { get; set; }
        public string? CompanyId { get; set; }
        public CompanyInfo? CompanyInfo { get; set; }
    }

    public class CompanyInfo
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }
    }
}

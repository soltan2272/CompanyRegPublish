using System.ComponentModel.DataAnnotations;

namespace CompanyDataLayer.Models
{
    public class OtpCodes
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = null!;
        
        [Required]
        [StringLength(6)]
        public string Code { get; set; } = null!;
        
        public DateTime ExpiryTime { get; set; }
        
        public bool IsUsed { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UsedAt { get; set; }
        
        // Navigation property
        public virtual ApplicationUser User { get; set; } = null!;
    }
}

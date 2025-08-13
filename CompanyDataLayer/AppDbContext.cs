using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyDataLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyDataLayer
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<OtpCodes> OtpCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationUser relationships
            builder.Entity<ApplicationUser>(entity =>
            {
                // Company-Employee relationship
                entity.HasOne(u => u.Company)
                      .WithMany(u => u.Employees)
                      .HasForeignKey(u => u.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict);

                // OTP relationship
                entity.HasMany<OtpCodes>()
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OtpCodes
            builder.Entity<OtpCodes>(entity =>
            {
                entity.HasIndex(o => o.UserId);
                entity.HasIndex(o => o.Code);
                entity.HasIndex(o => o.ExpiryTime);
            });
        }
    }
}

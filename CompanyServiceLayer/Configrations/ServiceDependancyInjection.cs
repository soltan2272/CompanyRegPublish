using System.Reflection;
using CompanyDataLayer.Models;
using CompanyServiceLayer.Interfaces;
using CompanyServiceLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.AspNetCore.Identity;


namespace CompanyServiceLayer.Configrations
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            // Register Email Service
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, EmailService>();

            // Register Caching
            services.AddMemoryCache();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register password hasher
            services.AddScoped<IPasswordHasher<Company>, PasswordHasher<Company>>();



            return services;
        }
    }

}

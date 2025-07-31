using System.Reflection;
using CompanyDataLayer.Models;
using CompanyServiceLayer.Interfaces;
using CompanyServiceLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace CompanyServiceLayer.Configrations
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, EmailService>();

            // Register Caching
            services.AddMemoryCache();


    

            return services;
        }
    }

}

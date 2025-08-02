
using System;
using CompanyDataLayer;
using Microsoft.EntityFrameworkCore;
using CompanyRepositoryLayer;
using CompanyServiceLayer.Configrations;

namespace CompanyApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        if (builder.Environment.IsDevelopment())
                        {
                            // Allow all in development for easier testing
                            policy.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                        }
                        else
                        {
                            // Production settings
                            policy.WithOrigins("https://yourproductiondomain.com")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                        }
                    });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRepositoryLayer(builder.Configuration);
            
            builder.Services.AddServiceLayer(builder.Configuration);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            var app = builder.Build();
            app.UseCors();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

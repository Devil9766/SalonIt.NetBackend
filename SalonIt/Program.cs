
using Microsoft.EntityFrameworkCore;
using SalonIt.Models;

namespace SalonIt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<SalonItContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("SalonDB")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()   // Allow all origins
                              .AllowAnyMethod()   // Allow all HTTP methods
                              .AllowAnyHeader();  // Allow all headers
                    });
            });

            var app = builder.Build();

            // Enable CORS
            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

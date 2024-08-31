using Airbnb.APIs.Extensions;
using Airbnb.APIs.Utility;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Use Extension Method To Add Services
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();
            // Apply Pending Migrations on Database
            await ExtensionMethods.ApplyMigrations(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

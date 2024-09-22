using Airbnb.APIs.Extensions;
using Airbnb.APIs.Utility;
using Newtonsoft.Json;

namespace Airbnb.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddHttpContextAccessor();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfigurations();

            await builder.Services.JWTConfigurations(builder.Configuration);
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
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

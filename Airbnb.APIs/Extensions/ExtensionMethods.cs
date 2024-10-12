using Airbnb.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Writers;

namespace Airbnb.APIs.Utility
{
    public static class ExtensionMethods
    {
        // To Create Data base if not exist
        public static async Task ApplyMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                // create object of Db context
                var service = scope.ServiceProvider;
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var _context = service.GetRequiredService<AirbnbDbContext>();
                    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
                    var unAppliedMigrations = await _context.Database.GetPendingMigrationsAsync();
                    if(unAppliedMigrations.Any())
                    {
                        await _context.Database.MigrateAsync();// Apply All pending migrations on database database
                    }
                  // can we call data seeding method here after we Apply all  pending migrations
                  await DataInitializer.SeedAsync(_context, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message);
                }
            }

        }
    }
}

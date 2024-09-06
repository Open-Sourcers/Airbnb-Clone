using Airbnb.APIs.Validators;
using Airbnb.Application.Services;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Airbnb.APIs.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<AirbnbDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            });
            // Identity Configurations
            // Identity Configurations
            Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AirbnbDbContext>()
                    .AddDefaultTokenProviders();

            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(typeof(CreateAccountValidator).Assembly);
            });

            return Services;
        }
    }
}

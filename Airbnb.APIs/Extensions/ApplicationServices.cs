using Airbnb.APIs.MiddelWairs;
using Airbnb.APIs.Validators;
using Airbnb.Application.Resolvers;
using Airbnb.Application.Services;
using Airbnb.Application.Settings;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace Airbnb.APIs.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<AirbnbDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RemoteConnection"));
                options.UseLazyLoadingProxies();
            });

            // Identity Configurations
            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<AirbnbDbContext>()
            .AddDefaultTokenProviders();

            // HttpContext Accessor

            Services.AddControllers();
            Services.AddMvc()
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                     options.SerializerSettings.Formatting = Formatting.Indented; 
                 });

            // AutoMapper Configuration
            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            Services.AddTransient<ExceptionMiddleWare>();
            Services.AddMemoryCache();
            Services.AddScoped<UserResolver>();
            Services.AddScoped<IPropertyService, PropertyService>();
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            Services.AddTransient<IMailService, MailService>();

            Services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(typeof(CreateAccountValidator).Assembly);
            });
           

            return Services;
        }
    }
}

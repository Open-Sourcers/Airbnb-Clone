using Airbnb.APIs.MiddelWairs;
using Airbnb.Application.Features.Bookings.Command.CreateBooking;
using Airbnb.Application.Resolvers;
using Airbnb.Application.Services;
using Airbnb.Application.Settings;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Interface;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace Airbnb.APIs.Extensions
{
	public static class ApplicationServices
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
		{
			#region Connections 
			Services.AddDbContext<AirbnbDbContext>(options =>
				{
					options.UseSqlServer(Configuration.GetConnectionString("RemoteConnection"));
					options.UseLazyLoadingProxies();
				});

			// Register all MediatR handlers from multiple assemblies
			Services.AddStackExchangeRedisCache(option =>
			{
				option.Configuration = Configuration.GetConnectionString("RedisConnection");

			});
			// Identity Configurations
			Services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				options.SignIn.RequireConfirmedEmail = true;
				options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
			})
			.AddEntityFrameworkStores<AirbnbDbContext>()
			.AddDefaultTokenProviders(); 
			#endregion


			Services.AddControllers();
			Services.AddMvc()
				 .AddNewtonsoftJson(options =>
				 {
					 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					 options.SerializerSettings.Formatting = Formatting.Indented;
				 });

			// AutoMapper Configuration
			Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			Services.AddMemoryCache();
			Services.AddScoped<UserResolver>();
			Services.AddScoped<IPropertyService, PropertyService>();
			Services.AddScoped<IAuthService, AuthService>();
			Services.AddScoped<IUserService, UserService>();
			Services.AddScoped<IUnitOfWork, UnitOfWork>();
			Services.AddScoped<IReviewService, ReviewServices>();
			Services.AddScoped<IReviewRepository, ReviewRepository>();
			Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
			Services.AddTransient<IMailService, MailService>();
			Services.AddTransient<ExceptionMiddleWare>();

			Services.AddMediatR(cgf =>
			cgf.RegisterServicesFromAssemblies(typeof(CreateBookingCommandHandler).Assembly)
			);

			Services.AddFluentValidation(fv =>
			{
				fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
			});


			return Services;
		}
	}
}

﻿using Airbnb.APIs.Validators;
using Airbnb.Application.Models;
using Airbnb.Application.Resolvers;
using Airbnb.Application.Services;
using Airbnb.Application.Services.Caching;
using Airbnb.Application.Settings;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Stripe;
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


            Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = Configuration.GetConnectionString("RedisRemote");
                return ConnectionMultiplexer.Connect(Connection);
            });
            // Identity Configurations
            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<AirbnbDbContext>()
            .AddDefaultTokenProviders();

            // AutoMapper Configuration
            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
            Services.AddMemoryCache();
            Services.AddScoped<UserResolver>();
            Services.AddScoped<IPropertyService,PropertyService>();
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IReviewService, ReviewServices>();
            Services.AddScoped<IBookService, BookService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IRedisCacheService, RedisCacheService>();
            Services.AddHttpContextAccessor();

            #region Payment configuration
            Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["StripeKeys:SecretKey"];

            Services.AddScoped<IPaymentService, PaymentService>();
            #endregion

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

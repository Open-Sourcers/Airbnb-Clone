using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace Airbnb.APIs.Extensions
{
    public static class AuthenticationServices
    {
        public static async Task JWTConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;// to make it an work at any protocol like http,https
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),

                        ValidateIssuer = true,
                        ValidIssuer = configuration["Token:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["Token:Audience"],

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // To Strict validation of token expiration
                    };
                });
        }
    }
}

using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Airbnb.Application.Resolvers
{
    public class UserResolver : IValueResolver<AppUser, UserDTO, string>
    {
        private readonly IConfiguration _configuration;

        public UserResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(AppUser source, UserDTO destination, string destMember, ResolutionContext context)
        {
            string Url = string.Empty;
            if(source.ProfileImage is not null)
            {
                Url = $"{_configuration["BaseUrl"]}{source.ProfileImage}";
            }
            return Url;
        }
    }
}

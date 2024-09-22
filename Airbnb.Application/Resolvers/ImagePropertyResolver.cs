using Airbnb.Domain.DataTransferObjects.Image;
using Airbnb.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Airbnb.Application.Resolvers
{
    public class ImagePropertyResolver : IValueResolver<Image, ImageResponse, string>
    {
        private readonly IConfiguration _configuration;

        public ImagePropertyResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Image source, ImageResponse destination, string destMember, ResolutionContext context)
        {
            string Url = string.Empty;
            if (source.Name != null)
            {
                Url = $"{_configuration["BaseUrl"]}{source.Name}";
            }
            return Url;
        }
    }
}

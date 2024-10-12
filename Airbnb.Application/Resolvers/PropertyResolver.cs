using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Airbnb.Application.Resolvers
{
    public class PropertyResolver : IValueResolver<Property, PropertyDTO, List<string>>
    {
        private readonly IConfiguration _configuration;

        public PropertyResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> Resolve(Property source, PropertyDTO destination, List<string> destMember, ResolutionContext context)
        {
            var Urls = new List<string>();
            if (source.Images != null && source.Images.Any())
            {
                foreach (var image in source.Images)
                {
                    if (!string.IsNullOrEmpty(image.Url))
                    {
                        var imageUrl = $"{_configuration["BaseUrl"]}{image.Url}";
                        Urls.Add(imageUrl);
                    }
                }
            }
            return Urls;
        }
    }

}

using Airbnb.Domain.DataTransferObjects.Property;
using Airbnb.Domain.Entities;
using AutoMapper;


namespace Airbnb.Application.MappingProfiler
{
    public class PropertyProfiler:Profile
    {
        public PropertyProfiler()
        {
            CreateMap<Property, PropertyResponse>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.Country, otp => otp.MapFrom(src => src.Location.Country.Name))
                .ForMember(dest => dest.Region, otp => otp.MapFrom(src => src.Location.Country.Region.Name));
          
            CreateMap<PropertyRequest, Property>();
                
        }
    }
}

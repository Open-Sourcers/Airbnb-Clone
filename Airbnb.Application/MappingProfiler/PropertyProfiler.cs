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
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NightPrice, opt => opt.MapFrom(src => src.NightPrice))
                .ForMember(dest => dest.PlaceType, opt => opt.MapFrom(src => src.PlaceType))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest=>dest.Country,otp=>otp.MapFrom(src=>src.Location.Country.Name))
                .ForMember(dest=>dest.Region,otp=>otp.MapFrom(src=>src.Location.Country.Region.Name))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
                .ForMember(dest => dest.Bookings, opt => opt.MapFrom(src => src.Bookings))
                .ForMember(dest => dest.RoomServices, opt => opt.MapFrom(src => src.RoomServices));

            CreateMap<PropertyRequest, Property>();
                
        }
    }
}

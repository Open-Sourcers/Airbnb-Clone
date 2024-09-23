
using Airbnb.Application.Resolvers;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.DataTransferObjects.Booking;
using Airbnb.Domain.DataTransferObjects.Category;
using Airbnb.Domain.DataTransferObjects.Country;
using Airbnb.Domain.DataTransferObjects.Image;
using Airbnb.Domain.DataTransferObjects.Location;
using Airbnb.Domain.DataTransferObjects.Property;
using Airbnb.Domain.DataTransferObjects.Region;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using AutoMapper;
using Org.BouncyCastle.Asn1;
namespace Airbnb.Application.MappingProfiler
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, UserDTO>()

            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Bookings, opt => opt.MapFrom(src => src.Bookings))
            .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
            .ForMember(dest => dest.profileImage, opt => opt.MapFrom<UserResolver>());

            CreateMap<Location, LocationResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Region,RegionResponse>()
                .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.Name));

            CreateMap<Country,CountryResponse>()
                .ForMember(dest=>dest.Name,opt=>opt.MapFrom(src=>src.Name));

            CreateMap<AppUser, OwnerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

           

            CreateMap<RoomService, RoomServicesDto>()
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Name));

            CreateMap<Booking, BookingResponse>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate));
                
            CreateMap<Image,ImageResponse>()
                .ForMember(dest=>dest.Url, otp=>otp.MapFrom<ImagePropertyResolver>());

            CreateMap<Review, ReviewDto>()
               .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Stars, opt => opt.MapFrom(src => src.Stars));

            CreateMap<Property, PropertyUserDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NightPrice, opt => opt.MapFrom(src => src.NightPrice))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.PlaceType, opt => opt.MapFrom(src => src.PlaceType));


            

            CreateMap<RegisterDTO, AppUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.MiddlName} {src.LastName}".Trim()))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

          }
    }
}

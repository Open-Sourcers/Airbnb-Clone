using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using Property = Airbnb.Domain.Entities.Property;

namespace Airbnb.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public PropertyService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<Responses> CreatePropertyAsync(string? email,PropertyDTO propertyDTO)
        {
            var owner = await _userManager.FindByEmailAsync(email);

            var region = await _unitOfWork.Repository<Region, int>().GetByIdAsync(propertyDTO.Region.Id);
            if (region == null)
            {
                var MappedRegion = new Region()
                {
                    Name = propertyDTO.Region.Name
                };

                await _unitOfWork.Repository<Region, int>().AddAsync(MappedRegion);
                var IsComplete = await _unitOfWork.CompleteAsync();
                if (IsComplete <= 0)
                {
                    return await Responses.FailurResponse("Region is not valid data!", HttpStatusCode.InternalServerError);
                }
            }

            var country = await _unitOfWork.Repository<Country, int>().GetByIdAsync(propertyDTO.Country.Id);
            if (country == null)
            {
                var MappedCountry = new Country()
                {
                    Name = propertyDTO.Country.Name,
                    RegionId = propertyDTO.Region.Id,
                };

                await _unitOfWork.Repository<Country, int>().AddAsync(MappedCountry);
                var IsComplete = await _unitOfWork.CompleteAsync();
                if (IsComplete <= 0)
                {
                    return await Responses.FailurResponse("Country is not valid data!", HttpStatusCode.InternalServerError);
                }
            }
           
            var location = await _unitOfWork.Repository<Location, int>().GetByIdAsync(propertyDTO.Location.Id);
            if (location == null)
            {
                var MappedLocation = new Location()
                {
                    Name = propertyDTO.Location.Name,
                    CountryId = propertyDTO.Country.Id
                };

                await _unitOfWork.Repository<Location, int>().AddAsync(MappedLocation);
                var IsComplete = await _unitOfWork.CompleteAsync();
                if (IsComplete <= 0)
                {
                    return await Responses.FailurResponse("Location is not valid data!", HttpStatusCode.InternalServerError);
                }
            }

            var images = new List<Image>();
            foreach(var i in propertyDTO.Images)
            {
                // upload image and take his url to assign it for object of images and add it in images list 
            }

            var roomServices = new List<RoomService>();
            foreach (var i in propertyDTO.Images)
            {
                // map every object from string to room service 
            }
            string PropertyId = Guid.NewGuid().ToString();
            var categories = new List<PropertyCategory>();
            foreach (var i in propertyDTO.Categories)
            {
                var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(i.Id);
                if (categories == null)
                {
                    return await Responses.FailurResponse(i, HttpStatusCode.NotFound);
                }
                categories.Add(new PropertyCategory()
                {
                    CategoryId = i.Id,
                    PropertyId = PropertyId
                }) ;
            }
            var MappedProperty = new Property()
            {
                Id = PropertyId,
                Name = propertyDTO.Name,
                Description = propertyDTO.Description,
                NightPrice = propertyDTO.NightPrice,
                PlaceType = propertyDTO.PlaceType,
                Location = location,
                Owner = owner,
                Images = images,
                Categories=categories,
                RoomServices = roomServices
            };
            await _unitOfWork.Repository<Property, string>().AddAsync(MappedProperty);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            
            return await Responses.SuccessResponse("Property has been created successfuly!");
        }
        public async Task<Responses> DeletePropertyAsync(string propertyId)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("There is no property with this id");
            _unitOfWork.Repository<Property, string>().Remove(property);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured");
            return await Responses.SuccessResponse("Property has been deleted successfully!");
        }
        public async Task<Responses> GetAllPropertiesAsync()
        {
            var properties = await _unitOfWork.Repository<Property, string>().GetAllAsync();
            if (properties.Any()) return await Responses.FailurResponse("There is no properties found", System.Net.HttpStatusCode.NotFound);
            return await Responses.SuccessResponse(properties);
        }
        public async Task<Responses> GetPropertyByIdAsync(string propertyId)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("Property is not found!", System.Net.HttpStatusCode.NotFound);
            return await Responses.SuccessResponse(property);
        }
        public async Task<Responses> UpdatePropertyAsync(string propertyId, PropertyDTO propertyDTO)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("Property is not found!", System.Net.HttpStatusCode.NotFound);

            property.Name = propertyDTO.Name;
            property.Description = propertyDTO.Description;
            property.NightPrice = propertyDTO.NightPrice;
            property.PlaceType = propertyDTO.PlaceType;
            //property.Location = propertyDTO.Location;
            property.Owner = await _userManager.FindByEmailAsync(propertyDTO.Owner.Email);
            //property.Images = propertyDTO.Images;
            //property.Categories = propertyDTO.Categories;

            _unitOfWork.Repository<Property, string>().Update(property);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.InternalServerError);
            return await Responses.SuccessResponse("Property has been updated successfully!");
        }


    }
}

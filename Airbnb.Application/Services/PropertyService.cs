﻿using Airbnb.Application.Settings;
using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net;
using Property = Airbnb.Domain.Entities.Property;

namespace Airbnb.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public PropertyService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<Responses> CreatePropertyAsync(string? email, PropertyToCreateDTO propertyDTO)
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

            string PropertyId = Guid.NewGuid().ToString();
            var images = new List<Image>();
            foreach (var img in propertyDTO.Images)
            {
                // upload image and take his url to assign it for object of images and add it in images list
                var ImgName = await DocumentSettings.UploadFile(img, SD.Image, "Property");
                var url = _configuration["BaseUrl"] + $"{ImgName}";
                var newImage = new Image()
                {
                    PropertyId = PropertyId,
                    Url = url
                };
                images.Add(newImage);
            }

            var roomServices = new List<RoomServicesToCreateDTO>();
            foreach (var item in propertyDTO.RoomServices)
            {
                // map every object from string to room service 
                var RS = new RoomServicesToCreateDTO()
                {
                    PropertyId = PropertyId,
                    Description = item.Description,
                };

                roomServices.Add(RS);
            }

            // var categories = new List<PropertyCategory>();
            //foreach (var i in propertyDTO.Categories)
            //{
            //    var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(i.Id);
            //    if (categories == null)
            //    {
            //        return await Responses.FailurResponse(i, HttpStatusCode.NotFound);
            //    }
            //    categories.Add(new PropertyCategory()
            //    {
            //        CategoryId = i.Id,
            //        PropertyId = PropertyId
            //    });
            //}
            var MappedRoomServices = _mapper.Map<ICollection<RoomServicesToCreateDTO>, ICollection<RoomService>>(roomServices);
            var MappedProperty = new Property()
            {
                Id = PropertyId,
                Name = propertyDTO.Name,
                Description = propertyDTO.Description,
                NightPrice = propertyDTO.NightPrice.Value,
                PlaceType = propertyDTO.PlaceType,
                Location = location,
                Owner = owner,
                //Images = images,
                // Categories = categories,
                //RoomServices = MappedRoomServices
            };
            await _unitOfWork.Repository<Property, string>().AddAsync(MappedProperty);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);

            await _unitOfWork.Repository<Image, int>().AddRangeAsync(images);
            await _unitOfWork.CompleteAsync();
            return await Responses.SuccessResponse("Property has been created successfuly!");
        }
        public async Task<Responses> DeletePropertyAsync(string propertyId)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("There is no property with this id");
            _unitOfWork.Repository<Property, string>().Remove(property);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured while removing");
            return await Responses.SuccessResponse("Property has been deleted successfully!");
        }
        public async Task<Responses> GetAllPropertiesAsync()
        {
            // there is a cycle when return the object
            var properties = (await _unitOfWork.Repository<Property, string>().GetAllAsync()).ToList();
            if (!properties.Any()) return await Responses.FailurResponse("There is no properties found", System.Net.HttpStatusCode.NotFound);
            var MappedProperties = _mapper.Map<List<Property>, List< PropertyDTO>>(properties);

            for(int i = 0; i < properties.Count(); i++)
            {
                var ImgUrls = new List<string>();
                foreach(var img in properties[i].Images)
                {
                    ImgUrls.Add(img.Url);
                }
                MappedProperties[i].ImageUrls = ImgUrls;
            }
            return await Responses.SuccessResponse(MappedProperties);
        }
        public async Task<Responses> GetPropertyByIdAsync(string propertyId)
        {
            // there is a cycle when return the object
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("Property is not found!", System.Net.HttpStatusCode.NotFound);
            var MappedProperty = _mapper.Map<Property, PropertyDTO>(property);
            var imagesUrl = new List<string>();
            foreach(var img in property.Images)
            {
                imagesUrl.Add(img.Url);
            }
            MappedProperty.ImageUrls = imagesUrl;
            return await Responses.SuccessResponse(MappedProperty);
        }
        public async Task<Responses> UpdatePropertyAsync(string propertyId, PropertyToUpdateDTO propertyDTO)
        {
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("Property is not found!", System.Net.HttpStatusCode.NotFound);
            // if the item is null shouldn't change anything.
            if(propertyDTO.Name is not null)
            property.Name = propertyDTO.Name;
            if (propertyDTO.Description is not null)
            property.Description = propertyDTO.Description;
            if(propertyDTO.NightPrice > 0)
            property.NightPrice = propertyDTO.NightPrice;
            if(propertyDTO.PlaceType is not null)
            property.PlaceType = propertyDTO.PlaceType;
            if(propertyDTO.Location is not null)
            property.Location.Id = propertyDTO.Location.Id;
            if(propertyDTO.Owner is not null)
            property.Owner = await _userManager.FindByEmailAsync(propertyDTO.Owner.Email);
            //property.Images = propertyDTO.Images;
            //property.PropertyCategories = propertyDTO.Categories;

            _unitOfWork.Repository<Property, string>().Update(property);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.InternalServerError);
            return await Responses.SuccessResponse("Property has been updated successfully!");
        }

    }
}

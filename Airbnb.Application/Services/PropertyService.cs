using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Cryptography;
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
        public async Task<Responses> CreatePropertyAsync(AppUser user,PropertyDTO propertyDTO)
        {
            var MappedProperty = new Property()
            {
                Name = propertyDTO.Name,
                Description = propertyDTO.Description,
                NightPrice = propertyDTO.NightPrice,
                PlaceType = propertyDTO.PlaceType,
                Location = propertyDTO.Location,
                Owner = user,
                Images = propertyDTO.Images,
                Categories = propertyDTO.Categories,
                RoomServices = propertyDTO.RoomServices
            };
            await _unitOfWork.Repository<Property, int>().AddAsync(MappedProperty);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            return await Responses.SuccessResponse("Property has been created successfuly!");
        }

        public async Task<Responses> DeletePropertyAsync(int propertyId)
        {
            var property = await _unitOfWork.Repository<Property, int>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("There is no property with this id");
            _unitOfWork.Repository<Property, int>().Remove(property);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse("Error has been occured");
            return await Responses.SuccessResponse("Property has been deleted successfully!");
        }

        public async Task<Responses> GetAllPropertiesAsync()
        {
            var properties = await _unitOfWork.Repository<Property, int>().GetAllAsync();
            if(properties.Any()) return await Responses.FailurResponse("There is no properties found", System.Net.HttpStatusCode.NotFound);
            return await Responses.SuccessResponse(properties);
        }

        public async Task<Responses> GetPropertyByIdAsync(int propertyId)
        {
            var property = await _unitOfWork.Repository<Property, int>().GetByIdAsync(propertyId);
            if(property == null) return await Responses.FailurResponse("Property is not found!", System.Net.HttpStatusCode.NotFound);
            return await Responses.SuccessResponse(property);
        }

        public async Task<Responses> UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO)
        {
            var property = await _unitOfWork.Repository<Property, int>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("Property is not found!", System.Net.HttpStatusCode.NotFound);

            property.Name = propertyDTO.Name;
            property.Description = propertyDTO.Description;
            property.NightPrice = propertyDTO.NightPrice;
            property.PlaceType = propertyDTO.PlaceType;
            property.Location = propertyDTO.Location;
            property.Owner = await _userManager.FindByEmailAsync(propertyDTO.Owner.Email);
            property.Images = propertyDTO.Images;
            property.Categories = propertyDTO.Categories;

            _unitOfWork.Repository<Property, int>().Update(property);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.InternalServerError);
            return await Responses.SuccessResponse("Property has been updated successfully!");
        }


    }
}

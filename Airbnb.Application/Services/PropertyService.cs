using Airbnb.Application.Settings;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Property;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Security.Claims;

namespace Airbnb.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public PropertyService(IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IHttpContextAccessor contextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        //TODO :Create Property
        public async Task<Responses> CreatePropertyAsync(PropertyRequest propertyDTO)
        {
            var owner = await GetCurrentUserAsync();
            var user = await _userManager.FindByEmailAsync(propertyDTO.OwnereEmail);
            if (owner == null || user == null || user.Email != owner.Email)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }
            var country = await _unitOfWork.Repository<Country, int>().GetByNameAsync(propertyDTO.Country);

            var Location = await _unitOfWork.Repository<Country, int>().GetByNameAsync(propertyDTO.Location);
            var Region = await _unitOfWork.Repository<Country, int>().GetByNameAsync(propertyDTO.Region);
            bool isThereCategoryFound = false;
            
            var Category = await _unitOfWork.Repository<Country, int>().GetByNameAsync(propertyDTO.Country);
            try
            {
                
                
                
                
            }
            catch(Exception ex)
            {

            }
            var images = new List<Image>();
            foreach (var i in propertyDTO.Images)
            {
                // upload image and take his url to assign it for object of images and add it in images list 
            }

            var roomServices = new List<RoomService>();
            foreach (var i in propertyDTO.Images)
            {
                // map every object from string to room service 
            }
            string PropertyId = Guid.NewGuid().ToString();

            var MappedProperty = new Property()
            {
                Id = PropertyId,
                Name = propertyDTO.Name,
                Description = propertyDTO.Description,
                NightPrice = propertyDTO.NightPrice,
                PlaceType = propertyDTO.PlaceType,
               // Location = propertyDTO,
                Owner = owner,
                Images = images,
                // Categories = categories,
                RoomServices = roomServices
            };
            await _unitOfWork.Repository<Property, string>().AddAsync(MappedProperty);
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);

            return await Responses.SuccessResponse("Property has been created successfuly!");
        }
        public async Task<Responses> DeletePropertyAsync(string propertyId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);
            if (property == null) return await Responses.FailurResponse("There is no property with this id");

            if (property.OwnerId != user.Id)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }
            try
            {
                _unitOfWork.Repository<Property, string>().Remove(property);
                // remove all images for this propertiy from server
                var Images = property.Images;
                foreach (var i in Images)
                {
                    var splitted = i.Name.Split('/');
                    if (splitted.Length == 4)
                    {
                        await DocumentSettings.DeleteFile(splitted[1], splitted[2], splitted[3]);
                    }
                }
                await _unitOfWork.CompleteAsync();
                return await Responses.SuccessResponse("Property has been deleted successfully!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.BadRequest);
            }

        }
        public async Task<Responses> GetAllPropertiesAsync()
        {
            // there is a cycle when return the object
            var properties = await _unitOfWork.Repository<Property, string>().GetAllAsync();

            if (!properties.Any()) return await Responses.FailurResponse("There is no properties found", HttpStatusCode.NotFound);

            return await Responses.SuccessResponse(_mapper.Map<IEnumerable<PropertyResponse>>(properties));
        }
        public async Task<Responses> GetPropertyByIdAsync(string propertyId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);

            if (property == null) return await Responses.FailurResponse("InValid propertyId ", HttpStatusCode.NotFound);

            if (property.OwnerId != user.Id)
            {
                return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);
            }

            return await Responses.SuccessResponse(_mapper.Map<PropertyResponse>(property));
        }
        public async Task<Responses> UpdatePropertyAsync(string propertyId, PropertyRequest propertyDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);
            }
            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(propertyId);

            if (property == null) return await Responses.FailurResponse("Property is not found!", HttpStatusCode.NotFound);

            if (property.OwnerId != user.Id)
            {
                return await Responses.FailurResponse("Unauthenticated request!", HttpStatusCode.Unauthorized);

            }
            try
            {
                var maped = _mapper.Map<Property>(propertyDTO);
                maped.Id = property.Id;
                _unitOfWork.Repository<Property, string>().Update(maped);

                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                return await Responses.FailurResponse(ex.Message.ToList(), HttpStatusCode.InternalServerError);
            }

            return await Responses.SuccessResponse("Property has been updated successfully!");
        }

        public async Task<AppUser>? GetCurrentUserAsync()
        {
            var userClaims = _contextAccessor.HttpContext?.User;

            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                return null;
            }

            var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(userEmail);

        }

    }
}

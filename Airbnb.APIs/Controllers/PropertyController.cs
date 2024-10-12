using System.Net;
using System.Security.Claims;
using Airbnb.Application.Services.Caching;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Azure;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
    public class PropertyController : APIBaseController
    {
        private readonly IPropertyService _propertyService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<PropertyDTO> _propertyValidator;
        private readonly IValidator<PropertyToCreateDTO> _propertyToCreateValidator;
        private readonly IRedisCacheService _redisCache;

        public PropertyController(IPropertyService propertyService, 
                                  UserManager<AppUser> userManager, 
                                  IValidator<PropertyDTO> propertyValidator,
                                  IValidator<PropertyToCreateDTO> propertyToCreateValidator,
                                  IRedisCacheService redisCache)
        {
            _propertyService = propertyService;
            _userManager = userManager;
            _propertyValidator = propertyValidator;
            _propertyToCreateValidator = propertyToCreateValidator;
            _redisCache = redisCache;
        }

        [HttpGet("GetProperties")]
        public async Task<ActionResult<Responses>> GetAllProperties()
        {
            var properties = await _redisCache.GetDataAsync<IEnumerable<PropertyDTO>>("properties");
            if(properties is not null)
            {
                var Result = Responses.SuccessResponse(properties);
                return Ok(Result);
            }
            var Response = await _propertyService.GetAllPropertiesAsync();
            await _redisCache.SetDataAsync("properties", Response.Data);
            return Ok(Response);
        }

        [HttpGet("GetProperty")]
        public async Task<ActionResult<Responses>> GetPropertyById(string? propertyId)
        {
            if(propertyId is null) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            var property = await _propertyService.GetPropertyByIdAsync(propertyId);
            return Ok(property);
        }
        //[Authorize(Roles = "Owner")]
        [HttpPost("CreateProperty")]
        public async Task<ActionResult<Responses>> CreateProperty([FromForm] PropertyToCreateDTO propertyDTO)
        {
            var validate = await _propertyToCreateValidator.ValidateAsync(propertyDTO);
            if (!validate.IsValid) return await Responses.FailurResponse(validate.Errors,HttpStatusCode.BadRequest);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = await _userManager.FindByIdAsync(userId);
            var email = User.FindFirstValue(ClaimTypes.Email);
            if(email is null)
            {
                return await Responses.FailurResponse("Owner is not found try again");
            }

            return Ok(Responses.SuccessResponse(await _propertyService.CreatePropertyAsync(email,propertyDTO)));
        }

        [HttpDelete("DeleteProperty")]
        public async Task<ActionResult<Responses>> DeleteProperty(string? propertyId)
        {
            if(propertyId is null) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            return Ok(Responses.SuccessResponse(await _propertyService.DeletePropertyAsync(propertyId)));
        }

        [HttpPut("UpdateProperty")]
        public async Task<ActionResult<Responses>> UpdateProperty([FromQuery] string? propertyId, [FromQuery] PropertyToUpdateDTO propertyDTO)
        {
            //var validate = await _propertyToCreateValidator.ValidateAsync(propertyDTO);
            //if(!validate.IsValid) return await Responses.FailurResponse(validate.Errors);
            if (propertyId is null) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            return Ok(await _propertyService.UpdatePropertyAsync(propertyId, propertyDTO));
        }

    }
}

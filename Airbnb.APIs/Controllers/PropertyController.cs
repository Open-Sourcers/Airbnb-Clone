using System.Net;
using System.Security.Claims;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
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

        public PropertyController(IPropertyService propertyService, UserManager<AppUser> userManager, IValidator<PropertyDTO> propertyValidator)
        {
            _propertyService = propertyService;
            _userManager = userManager;
            _propertyValidator = propertyValidator;
        }

        [HttpGet("GetProperties")]
        public async Task<ActionResult<Responses>> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
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
        public async Task<ActionResult<Responses>> CreateProperty(PropertyDTO propertyDTO)
        {
            var validate = await _propertyValidator.ValidateAsync(propertyDTO);
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
        public async Task<ActionResult<Responses>> UpdateProperty(string? propertyId, PropertyDTO propertyDTO)
        {
            var validate = await _propertyValidator.ValidateAsync(propertyDTO);
            if(!validate.IsValid) return await Responses.FailurResponse(validate.Errors);
            if (propertyId is null) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            return Ok(await _propertyService.UpdatePropertyAsync(propertyId, propertyDTO));
        }

    }
}

using System.Net;
using System.Security.Claims;
using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Property;
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
        private readonly IValidator<PropertyRequest> _PropertyRequest;

        public PropertyController(IPropertyService propertyService, UserManager<AppUser> userManager, IValidator<PropertyRequest> propertyRequest)

        {
            _propertyService = propertyService;
            _userManager = userManager;
            _PropertyRequest = propertyRequest;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetProperties")]
        public async Task<ActionResult<Responses>> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        [Authorize(Roles="Owner")]
        [Authorize(Roles="Admin")]
        [HttpGet("GetProperty/{propertyId}")]
        public async Task<ActionResult<Responses>> GetPropertyById(string propertyId)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user == null)
            {
                return await Responses.FailurResponse("UnAuthorized", HttpStatusCode.Unauthorized);
            }
            return Ok(await _propertyService.GetPropertyByIdAsync(propertyId));
        }

        [Authorize(Roles = "Owner")]
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateProperty")]
        public async Task<ActionResult<Responses>> CreateProperty(PropertyRequest propertyDTO)
        {
            var validate = await _PropertyRequest.ValidateAsync(propertyDTO);
            if (!validate.IsValid) return await Responses.FailurResponse(validate.Errors, HttpStatusCode.BadRequest);

            return Ok(Responses.SuccessResponse(await _propertyService.CreatePropertyAsync(propertyDTO)));
        }

        [HttpDelete("DeleteProperty/{propertyId}")]
        public async Task<ActionResult<Responses>> DeleteProperty([FromRoute]string propertyId)
        {
            return Ok(Responses.SuccessResponse(await _propertyService.DeletePropertyAsync(propertyId)));
        }

        [HttpPut("UpdateProperty")]
        public async Task<ActionResult<Responses>> UpdateProperty(string? propertyId, PropertyRequest propertyDTO)
        {
            var validate = await _PropertyRequest.ValidateAsync(propertyDTO);
            if (!validate.IsValid) return await Responses.FailurResponse(validate.Errors);
            if (propertyId is null) return await Responses.FailurResponse(System.Net.HttpStatusCode.BadRequest);
            return Ok(await _propertyService.UpdatePropertyAsync(propertyId, propertyDTO));
        }

    }
}

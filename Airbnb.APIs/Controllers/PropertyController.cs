using System.Net;
using System.Security.Claims;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Property;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Interface;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Specifications;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Airbnb.APIs.Controllers
{
    public class PropertyController : APIBaseController
    {
        private readonly IPropertyService _propertyService;
        private readonly IValidator<PropertyRequest> _PropertyRequest;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IValidator<PropertyRequest> propertyRequest, IUnitOfWork unitOfWork, IMapper mapper)

        {
            _propertyService = propertyService;
            _PropertyRequest = propertyRequest;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetProperties")]
        public async Task<ActionResult<Responses>> GetAllProperties([FromQuery]ProductSpecParameters param)
        {
            var spec=new PropertyWithSpec(param);
            var specs = await _unitOfWork.Repository<Property, string>().GetAllWithSpecAsync(spec)!;
            var maped = _mapper.Map<IEnumerable<PropertyResponse>>(specs);

			return Ok(maped);
        }

        [Authorize(Roles = "Owner")]
        [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<Responses>> CreateProperty([FromForm] PropertyRequest propertyDTO)
        {
            var validate = await _PropertyRequest.ValidateAsync(propertyDTO);
            if (!validate.IsValid) return await Responses.FailurResponse(validate.Errors, HttpStatusCode.BadRequest);

            return Ok(Responses.SuccessResponse(await _propertyService.CreatePropertyAsync(propertyDTO)));
        }

        [HttpDelete("DeleteProperty/{propertyId}")]
        public async Task<ActionResult<Responses>> DeleteProperty([FromRoute] string propertyId)
        {
            return Ok(Responses.SuccessResponse(await _propertyService.DeletePropertyAsync(propertyId)));
        }

        [HttpPut("UpdateProperty")]
        // TODO: un Completed Implementation for this method
        // TODO: the is some Errors in Authorization
        public async Task<ActionResult<Responses>> UpdateProperty([FromForm]UpdatePropertyDto propertyDTO)
        {
            if (propertyDTO.PropertyId is null) return await Responses.FailurResponse("Invalid nullable Id",HttpStatusCode.BadRequest);
            return Ok(await _propertyService.UpdatePropertyAsync(propertyDTO));
        }

    }
}

using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airbnb.Domain;
using Airbnb.Infrastructure.Specifications;
using Airbnb.Domain.Interfaces.Repositories;
using AutoMapper;
using Airbnb.Domain.DataTransferObjects;
using FluentValidation;
using Airbnb.APIs.Validators;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Airbnb.Domain.Identity;

namespace Airbnb.APIs.Controllers
{

    public class ReviewController : APIBaseController
    {
        private readonly IReviewService _reviewService;
        private readonly IValidator<ReviewDto> _validator;
        private readonly UserManager<AppUser> _userManager;
        public ReviewController(IReviewService reviewService,
            IValidator<ReviewDto> validator
,
            UserManager<AppUser> userManager)

        {
            _reviewService = reviewService;
            _validator = validator;
            _userManager = userManager;
        }

        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<Responses>> GetAllReviews(string? propertyId, string? userId)
        {
            return Ok(await _reviewService.GetAllReviewsAsync(propertyId, userId));
        }


        [HttpGet("GetReviewDetails")]
        public async Task<ActionResult<Responses>> GetReview(int id)
        {
            return Ok(await _reviewService.GetReviewAsync(id));
        }

        [HttpPost("CreateReview")]
        public async Task<ActionResult<Responses>> AddReview([FromBody] ReviewDto review)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var validation = await _validator.ValidateAsync(review);
            if (!validation.IsValid || user.Id!= review.UserId)
            {
                return BadRequest(await Responses.FailurResponse(ModelState));
            }
            return Ok(await _reviewService.AddReviewAsync(review));

        }

        [HttpPut("UpdateReview/{id}")]
        public async Task<ActionResult<Responses>> UpdateReview(int id, [FromBody] ReviewDto review)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var validation = await _validator.ValidateAsync(review);
            if (review.UserId != user.Id)
            {
                return await Responses.FailurResponse("UnAuthenticated user!", HttpStatusCode.Unauthorized);
            }
            if (!validation.IsValid)
            {
                return await Responses.FailurResponse(validation.Errors,HttpStatusCode.BadRequest);
            }
            return Ok(await _reviewService.UpdateReviewAsync(user.Id,id,review));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Responses>> DeleteReview(int id)
        {
            return Ok(await _reviewService.DeleteReviewAsync(id));
        }

    }
}

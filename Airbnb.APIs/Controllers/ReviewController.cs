using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airbnb.Domain;

namespace Airbnb.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            if (reviews != null)
            {
                return Ok(await Responses.SuccessResponse(reviews, "Reviews retrieved successfully."));
            }
            return NotFound(await Responses.FailurResponse("No reviews found."));
        }

        // GET: api/Review/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _reviewService.GetReviewAsync(id);
            if (review != null)
            {
                return Ok(await Responses.SuccessResponse(review, "Review retrieved successfully."));
            }
            return NotFound(await Responses.FailurResponse($"Review with ID {id} not found."));
        }

        // POST: api/Review
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(await Responses.FailurResponse(ModelState));
            }

            await _reviewService.AddReviewAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id },
                await Responses.SuccessResponse(review, "Review created successfully."));
        }

        // PUT: api/Review/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(await Responses.FailurResponse(ModelState));
            }

            var existingReview = await _reviewService.GetReviewAsync(id);
            if (existingReview == null)
            {
                return NotFound(await Responses.FailurResponse($"Review with ID {id} not found."));
            }

            // Replace with Update method if implemented
            await _reviewService.AddReviewAsync(review);
            return Ok(await Responses.SuccessResponse(review, "Review updated successfully."));
        }

        // DELETE: api/Review/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewService.GetReviewAsync(id);
            if (review == null)
            {
                return NotFound(await Responses.FailurResponse($"Review with ID {id} not found."));
            }

            await _reviewService.DeleteReviewAsync(id);
            return Ok(await Responses.SuccessResponse(null, "Review deleted successfully."));
        }

        // GET: api/Review/Property/{propertyId}
        [HttpGet("Property/{propertyId}")]
        public async Task<IActionResult> GetReviewsByProperty(string propertyId)
        {
            var reviews = await _reviewService.GetReviewsByPropertyIdAsync(propertyId);
            if (reviews != null && reviews.Any())
            {
                return Ok(await Responses.SuccessResponse(reviews, $"Reviews for Property ID {propertyId} retrieved successfully."));
            }
            return NotFound(await Responses.FailurResponse($"No reviews found for Property ID {propertyId}."));
        }
    }
}

using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/Review/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _reviewService.GetReviewAsync(id);
            if (review == null)
            {
                return NotFound(new { Message = $"Review with ID {id} not found." });
            }
            return Ok(review);
        }

        // POST: api/Review
        [HttpPost]
        public async Task<ActionResult> AddReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _reviewService.AddReviewAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // PUT: api/Review/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingReview = await _reviewService.GetReviewAsync(id);
            if (existingReview == null)
            {
                return NotFound(new { Message = $"Review with ID {id} not found." });
            }

            await _reviewService.AddReviewAsync(review);  // Replace with Update if implemented
            return NoContent();
        }

        // DELETE: api/Review/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var review = await _reviewService.GetReviewAsync(id);
            if (review == null)
            {
                return NotFound(new { Message = $"Review with ID {id} not found." });
            }

            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }

        // GET: api/Review/Property/{propertyId}
        [HttpGet("Property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByProperty(int propertyId)
        {
            var reviews = await _reviewService.GetReviewsByPropertyIdAsync(propertyId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound(new { Message = $"No reviews found for Property ID {propertyId}." });
            }
            return Ok(reviews);
        }
    }
}

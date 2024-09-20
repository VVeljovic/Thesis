using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccommodationService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private IReviewService _reviewService;

        public ReviewController (IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewDto reviewDto)
        {
            var result = await _reviewService.CreateReviewAsync(reviewDto);
            return Ok(result);
        }

         [HttpGet("get-reviews/{accommodationId}/{pageSize}/{pageNumber}")]
        public async Task<IActionResult> GetReviewsAsync(string accommodationId, int pageSize, int pageNumber)
        {
            var reviewsDto = await _reviewService.GetReviewsFromAccommodationAsync(accommodationId,pageSize,pageNumber);
            return Ok(reviewsDto);
        }
    }
}

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
    }
}

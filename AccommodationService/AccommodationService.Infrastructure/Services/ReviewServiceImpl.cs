using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Infrastructure.MongoDb;

namespace AccommodationService.Infrastructure.Services
{
    public class ReviewServiceImpl : IReviewService
    {

        private readonly ReviewContext _reviewDbContext;

        public ReviewServiceImpl(ReviewContext reviewDbContext)
        {
            _reviewDbContext = reviewDbContext;
        }
        public async Task<ReviewDto> CreateReviewAsync(ReviewDto reviewDto)
        {
            var review = ReviewDto.MapReviewDtoToReview(reviewDto);
            await _reviewDbContext.CreateReviewAsync(review);
            return reviewDto;
        }

        public Task<IEnumerable<ReviewDto>> GetReviewsFromAccommodationAsync()
        {
            throw new NotImplementedException();
        }
    }
}
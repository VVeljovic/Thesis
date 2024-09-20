using AccommodationService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Application.Interfaces
{
    public interface IReviewService
    {
        public Task<ReviewDto> CreateReviewAsync(ReviewDto reviewDto);

        public Task<IEnumerable<ReviewDto>> GetReviewsFromAccommodationAsync(string accommodationId, int pageSize, int pageNumber);
    }
}

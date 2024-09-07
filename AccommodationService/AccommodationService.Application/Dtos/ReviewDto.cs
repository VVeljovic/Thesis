using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationService.Domain.Models;

namespace AccommodationService.Application.Dtos
{
    public class ReviewDto
    {
        public string UserId { get; set; }

        public string AccommodationId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime DateCreated { get; set; }

        public static Review MapReviewDtoToReview(ReviewDto reviewDto)
        {
            return new Review()
            {
                UserId = reviewDto.UserId,
                AccommodationId = reviewDto.AccommodationId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                DateCreated=reviewDto.DateCreated
            };
            
        }

    }
}

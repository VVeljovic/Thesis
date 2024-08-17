using AccommodationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Application.Dtos
{
    public class AccommodationDto
    {
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public List<String> Photos { get; set; }
        public string UserId { get; set; }

        public AmenityDto Amenity { get; set; }

        public static Accommodation mapDtoToAccommodation(AccommodationDto acommodationDto)
        {
            return new Accommodation
            {
                Id = Guid.NewGuid().ToString(),
                PropertyName = acommodationDto.PropertyName,
                Description = acommodationDto.Description,
                Address = acommodationDto.Address,
                PricePerNight = acommodationDto.PricePerNight,
                AvailableFrom = acommodationDto.AvailableFrom,
                AvailableTo = acommodationDto.AvailableTo,
                Photos = acommodationDto.Photos,
                UserId = acommodationDto.UserId
            };
        }
    }
}

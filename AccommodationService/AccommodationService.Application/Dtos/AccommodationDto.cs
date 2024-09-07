using AccommodationService.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;
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
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public AmenityDto Amenity { get; set; }

        public static Accommodation MapDtoToAccommodation(AccommodationDto acommodationDto)
        {
            return new Accommodation
            {
                PropertyName = acommodationDto.PropertyName,
                Description = acommodationDto.Description,
                Address = acommodationDto.Address,
                PricePerNight = acommodationDto.PricePerNight,
                AvailableFrom = acommodationDto.AvailableFrom,
                AvailableTo = acommodationDto.AvailableTo,
                Photos = acommodationDto.Photos,
                UserId = acommodationDto.UserId,
                Location =  new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                    new GeoJson2DGeographicCoordinates(acommodationDto.Longitude,acommodationDto.Latitude))
                
            };
        }

        public static AccommodationDto MapAccommodationToDto(Accommodation accommodation)
        {
            return new AccommodationDto
            {
                PropertyName = accommodation.PropertyName,
                Description = accommodation.Description,
                Address = accommodation.Address,
                PricePerNight = accommodation.PricePerNight,
                AvailableFrom = accommodation.AvailableFrom,
                AvailableTo = accommodation.AvailableTo,
                Photos = accommodation.Photos,
                UserId = accommodation.UserId,
                Longitude = accommodation.Location.Coordinates.Longitude,
                Latitude = accommodation.Location.Coordinates.Latitude
               
            };
        }
    }
}

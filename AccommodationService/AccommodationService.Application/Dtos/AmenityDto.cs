using AccommodationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Application.Dtos
{
    public class AmenityDto
    {
        public bool? Parking { get; set; }

        public bool? WiFi { get; set; }

        public bool? PetsAllowed { get; set; }

        public bool? SwimmingPool { get; set; }

        public bool? Spa { get; set; }

        public bool? FitnessCentre { get; set; }

        public bool? NonSmokingRooms { get; set; }

        public bool? RoomService { get; set; }

        public bool? Balcony { get; set; }

        public bool? Television { get; set; }

        public bool? AirConditioning { get; set; }

        public static Amenity MapToAmenity(AmenityDto amenityDto)
        {
            return new Amenity()
            {
                Parking = amenityDto.Parking,
                WiFi = amenityDto.WiFi,
                PetsAllowed = amenityDto.PetsAllowed,
                SwimmingPool = amenityDto.SwimmingPool,
                Spa = amenityDto.Spa,
                FitnessCentre = amenityDto.FitnessCentre,
                NonSmokingRooms = amenityDto.NonSmokingRooms,
                RoomService = amenityDto.RoomService,
                Balcony = amenityDto.Balcony,
                Television = amenityDto.Television,
                AirConditioning = amenityDto.AirConditioning
            };
        }

        public static AmenityDto MapToAmenityDto(Amenity amenity)
        {
            return new AmenityDto()
            {
                Parking = amenity.Parking,
                WiFi = amenity.WiFi,
                PetsAllowed = amenity.PetsAllowed,
                SwimmingPool = amenity.SwimmingPool,
                Spa = amenity.Spa,
                FitnessCentre = amenity.FitnessCentre,
                NonSmokingRooms = amenity.NonSmokingRooms,
                RoomService = amenity.RoomService,
                Balcony = amenity.Balcony,
                Television = amenity.Television,
                AirConditioning = amenity.AirConditioning
            };
        }
    }
}

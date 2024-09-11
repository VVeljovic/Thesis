using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Domain.Models
{
    public class Accommodation
    {
        public ObjectId Id { get; set; }

        public string PropertyName { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public decimal PricePerNight { get; set; }

        public decimal NumberOfGuests { get; set; }

        public DateTime AvailableFrom { get; set; }

        public DateTime AvailableTo { get; set; }

        public List<String> Photos { get; set; }

        public string UserId { get; set; }

        public Amenity Amenity { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        public List<Review> LastFiveReviews { get; set; } = new List<Review>(5);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Domain.Models
{
    public class Amenity
    {
        public bool? Parking { get; set; }
        public bool? WiFi { get; set; }
        public bool? PetsAllowed { get; set; }
        public bool? SwimmingPool { get; set; }
        public bool? Spa { get; set; }
        public bool? FitnessCentre { get; set; }
        public bool? NonSmokingRooms { get; set; }
        public bool? RoomService { get; set; }
    }
}

namespace AccomodationService.Models
{
    public class AccommodationModel
    {
        public Guid Id { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }    
        public string Address { get; set; }    
        public decimal PricePerNight { get; set; }   
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public List<String> Photos { get; set; }
        public string UserId { get; set;  }
        //Facilities
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

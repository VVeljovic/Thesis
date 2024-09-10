using MongoDB.Bson;

public class Reservation
{
    public ObjectId Id { get; set;}

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public double TotalAmount { get; set; }

    public string Status { get; set; }

    public string AccommodationId { get; set; }
}
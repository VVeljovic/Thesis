public class ReservationDto
{
    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public double TotalAmount { get; set; }

    public string Status { get; set; }

    public string AccommodationId { get; set; }

    public string UserId { get; set; }

    public static ReservationDto MapReservationToDto(Reservation reservation)
    {
        return new ReservationDto()
        {
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            TotalAmount = reservation.TotalAmount,
            Status = reservation.Status,
            AccommodationId = reservation.AccommodationId,
            UserId = reservation.UserId
        };
    }
}
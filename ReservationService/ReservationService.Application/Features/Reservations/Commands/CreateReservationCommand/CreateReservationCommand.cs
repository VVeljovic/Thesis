using MediatR;
public class CreateReservationCommand : IRequest<CreateReservationCommand>
{
    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public double TotalAmount { get; set; }

    public string Status { get; set; }

    public string AccommodationId { get; set; }

    public static Reservation MapToReservation(CreateReservationCommand createReservationCommand)
    {
        return new Reservation()
        {
            CheckInDate = createReservationCommand.CheckInDate,
            CheckOutDate = createReservationCommand.CheckOutDate,
            TotalAmount = createReservationCommand.TotalAmount,
            Status = createReservationCommand.Status,
            AccommodationId = createReservationCommand.AccommodationId
        };
    }
}
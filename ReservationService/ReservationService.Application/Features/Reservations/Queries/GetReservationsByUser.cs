using MediatR;

public class GetReservationsByUser  : IRequest<IEnumerable<ReservationDto>>
{
    public string UserId { get; set; }
}
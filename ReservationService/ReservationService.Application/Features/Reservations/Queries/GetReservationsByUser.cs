using System.ComponentModel;
using MediatR;

public class GetReservationsByUser  : IRequest<IEnumerable<ReservationDto>>
{
    public string UserId { get; set; }
    
    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public string Status { get; set; }

    
}
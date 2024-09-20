using MediatR;

public class GetReservationByUserHandler : IRequestHandler<GetReservationsByUser, IEnumerable<ReservationDto>>
{
    public IReservationService _reservationService { get; set; }

    public GetReservationByUserHandler(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    async Task<IEnumerable<ReservationDto>> IRequestHandler<GetReservationsByUser, IEnumerable<ReservationDto>>.Handle(GetReservationsByUser request, CancellationToken cancellationToken)
    {
         return await  _reservationService.GetReservationsByUserId(request.UserId);
    }
} 
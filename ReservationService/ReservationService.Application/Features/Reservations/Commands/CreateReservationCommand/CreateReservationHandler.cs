using MediatR;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, CreateReservationCommand>
{
    public readonly IReservationService _reservationService;
    public CreateReservationHandler(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    public Task<CreateReservationCommand> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        _reservationService.CreateReservationAsync(request);
        return Task.FromResult(request);
    }
}
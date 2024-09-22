public interface IReservationService
{
    public Task CreateReservationAsync(CreateReservationCommand createReservationCommand);

    public Task<IEnumerable<ReservationDto>> GetReservationsByUserId(string userId, int pageSize=5, int pageNumber=1, string status="Success");

    public Task HandleMessageAsync<T>(T message, string queueName);
}
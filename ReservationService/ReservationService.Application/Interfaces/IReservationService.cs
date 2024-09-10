public interface IReservationService
{
    public Task CreateReservationAsync(CreateReservationCommand createReservationCommand);

    public Task HandleMessageAsync<T>(T message, string queueName);
}
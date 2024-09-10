
using System.Text.Json;

public class ReservationService : IReservationService
{
    private ReservationContext _reservationContext;

    public ReservationService(ReservationContext reservationContext)
    {
        _reservationContext = reservationContext;
    }
    public async Task CreateReservationAsync(CreateReservationCommand createReservationCommand)
    {
        var reservation = CreateReservationCommand.MapToReservation(createReservationCommand);
        await _reservationContext.InsertReservationAsync(reservation);
    }

    public Task HandleMessageAsync<T>(T message, string queueName)
    {
        var messageJson = JsonSerializer.Serialize(message);

        Console.WriteLine($"Received message from queue: {queueName}");
        Console.WriteLine("Message content:");
        Console.WriteLine(messageJson);

        return Task.CompletedTask;
    }
}
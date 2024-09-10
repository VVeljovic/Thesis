using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;

public class RabbitMQConsumerHostedService<T> : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQConsumerHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var consumer = scope.ServiceProvider.GetRequiredService<IRabbitMQConsumer<T>>();
            consumer.StartConsuming(HandleMessage);
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleMessage(T message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<IAccommodationService>();
            await handler.HandleMessageAsync(message);
        }
    }
}

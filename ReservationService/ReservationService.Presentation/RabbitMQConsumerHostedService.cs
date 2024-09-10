
public class RabbitMQConsumerHostedService<T> : IHostedService
{
    private readonly IRabbitMQConsumer<T> _consumer;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMQConsumerHostedService(IRabbitMQConsumer<T> consumer, IServiceProvider serviceProvider)
    {
        _consumer = consumer;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.StartConsuming(HandleMessage);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleMessage(T message, string queueName)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<IReservationService>();
            await handler.HandleMessageAsync(message, queueName);
        }
    }
}

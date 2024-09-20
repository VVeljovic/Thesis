
using Microsoft.AspNetCore.SignalR;
using TransactionService.Application.Interfaces;

public class RabbitMQConsumerHostedService<T> : IHostedService
{
    private readonly IRabbitMQConsumer<T> _consumer;
    private readonly IServiceProvider _serviceProvider;

    private readonly IHubContext<NotificationService, INotificationService> _context;

    public RabbitMQConsumerHostedService(IRabbitMQConsumer<T> consumer, IServiceProvider serviceProvider, IHubContext<NotificationService, INotificationService> context)
    {
        _consumer = consumer;
        _serviceProvider = serviceProvider;
        _context = context;
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
            var handler = scope.ServiceProvider.GetRequiredService<ITransactionService>();
            string messageResponse = await handler.HandleMessageAsync(message, queueName);
            await _context.Clients.All.ReceiveNotification(messageResponse);
        }
    }
}

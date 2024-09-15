using AccommodationService.Application.Dtos;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMQQueues>();
        services.AddSingleton(provider =>
        {
            return new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,

            };
        });

        services.AddSingleton(provider =>
         {
             var connectionFactory = provider.GetRequiredService<ConnectionFactory>();
             return connectionFactory.CreateConnection();
         });
        services.AddSingleton(provider =>
        {
            var connection = provider.GetRequiredService<IConnection>();
            return connection.CreateModel();
        });
        services.AddScoped(typeof(IRabbitMQProducer<>), typeof(RabbitMQProducer<>));
        services.AddScoped<IReservationService, ReservationService>();
        services.AddSingleton<ReservationContext>();
        services.AddSingleton<IRabbitMQConsumer<TransactionRequestDto>>(sp =>
        {
            var channel = sp.GetRequiredService<IModel>();
            var queues = sp.GetRequiredService<RabbitMQQueues>();
            return new RabbitMQConsumer<TransactionRequestDto>(channel, queues.TransactionRequestQueues);
        });

        services.AddSingleton<IRabbitMQConsumer<TransactionResponseDto>>(sp =>
        {
            var channel = sp.GetRequiredService<IModel>();
            var queues = sp.GetRequiredService<RabbitMQQueues>();
            return new RabbitMQConsumer<TransactionResponseDto>(channel, queues.TransactionResponseQueues);
        });
        return services;
    }
}
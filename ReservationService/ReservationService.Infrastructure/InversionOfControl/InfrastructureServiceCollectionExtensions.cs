using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
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
        services.AddSingleton(typeof(IRabbitMQConsumer<>), typeof(RabbitMQConsumer<>));
        services.AddScoped<IReservationService, ReservationService>();
        services.AddSingleton<ReservationContext>();
        return services;
    }
}
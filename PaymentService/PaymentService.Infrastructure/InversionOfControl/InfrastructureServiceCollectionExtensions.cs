
using AccommodationService.Application.Dtos.ChoreographyDtos;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
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
        services.AddScoped<ChargeService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<TokenService>();
        services.AddScoped(typeof(IRabbitMQProducer<>), typeof(RabbitMQProducer<>));
        services.AddSingleton<IRabbitMQConsumer<TransactionRequestDto>>(sp =>
          {
              var channel = sp.GetRequiredService<IModel>();
              var queues = sp.GetRequiredService<RabbitMQQueues>();
              return new RabbitMQConsumer<TransactionRequestDto>(channel, queues.TransactionRequestQueues);
          });

        services.AddScoped<IPaymentService, PaymentServiceImpl>();

        return services;
    }
}


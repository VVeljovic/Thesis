using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Interfaces;
using TransactionService.Infrastructure.MongoDb;
using TransactionService.Infrastructure.Services;

namespace TransactionService.Infrastructure.InversionOfControl
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            {
                services.AddScoped<ITransactionService, TransactionServiceImpl>();
                services.AddSingleton<TransactionDbContext>();
                services.AddSingleton(provider =>
                {
                    return new ConnectionFactory
                    {
                        HostName = "localhost",
                        Port = 5672
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
                services.AddScoped(typeof(IRabbitMQProducer<>), typeof(RabbitMQProducer<>));
                
                return services;
            }
        }
    }
}

using AccommodationService.Application.Dtos;
using AccommodationService.Application.Dtos.ChoreographyDtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Infrastructure.MongoDb;
using AccommodationService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace AccommodationService.Infrastructure.InversionOfControl
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IReviewService, ReviewServiceImpl>();
            services.AddSingleton<AccommodationContext>();
            services.AddSingleton<ReviewContext>();
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

            services.AddScoped<IAccommodationService, AccommodationServiceImpl>();
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
}

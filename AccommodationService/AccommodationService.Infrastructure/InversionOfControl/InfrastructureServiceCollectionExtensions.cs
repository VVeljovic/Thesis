using AccommodationService.Application.Dtos;
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
            services.AddScoped<IAccommodationService, AccommodationServiceImpl>();
            services.AddScoped(typeof(IRabbitMQProducer<>), typeof(RabbitMQProducer<>));

            return services;
        }
    }
}

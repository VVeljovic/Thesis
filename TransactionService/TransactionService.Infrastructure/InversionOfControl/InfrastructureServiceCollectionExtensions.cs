using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
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
                services.AddSingleton<IProducer<Null, string>>(provider =>
                {
                    var producerConfig = new ProducerConfig
                    {
                        BootstrapServers = "localhost:29092",
                    };
                    return new ProducerBuilder<Null, string>(producerConfig).Build();
                });
                services.AddScoped<IProducer, ProducerImpl>();
                services.AddScoped<ITransactionService, TransactionServiceImpl>();
                services.AddSingleton<TransactionDbContext>();
                return services;
            }
        }
    }
}

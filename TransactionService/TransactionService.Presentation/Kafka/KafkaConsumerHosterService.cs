
using TransactionService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionService.Presentation.Kafka
{
    public class KafkaConsumerHosterService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumerHosterService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();

                await transactionService.ReadMessageFromTopic(stoppingToken);
            }
        }
    }
}

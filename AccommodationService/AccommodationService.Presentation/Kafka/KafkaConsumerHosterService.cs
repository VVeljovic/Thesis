
using AccommodationService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationService.Presentation.Kafka
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
                var accommodationService = scope.ServiceProvider.GetRequiredService<IAccommodationService>();

                await accommodationService.ExecuteAsync(stoppingToken);
            }
        }
    }
}

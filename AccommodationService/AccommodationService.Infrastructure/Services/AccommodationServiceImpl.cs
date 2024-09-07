using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Domain.Models;
using AccommodationService.Infrastructure.MongoDb;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace AccommodationService.Infrastructure.Services
{
    public class AccommodationServiceImpl : IAccommodationService
    {
        private readonly AccommodationContext _dbContext;

        private readonly IConsumer<Ignore, string> _consumer;


        public AccommodationServiceImpl(AccommodationContext dbContext)
        {
            _dbContext = dbContext;
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:29092",
                GroupId = "Inventory",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _consumer.Subscribe("topiccc");
        }

        public Task<ReviewDto> CreateReview(ReviewDto reviewDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccommodation(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccommodationDto> GetAccommodationByIdAsync(string id)
        {
            var accommodationDto = await _dbContext.GetAccommodationByIdAsync(id);
            return accommodationDto;
        }

        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsAsync(double longitude,double latitude, int pageSize, int pageNumber)
        {
          return await _dbContext.GetAccommodationsAsync(longitude, latitude, pageSize, pageNumber); 
        }

        public async Task<IEnumerable<AccommodationDto>> GetMyAccommodationsAsync(string userId)
        {
            return await _dbContext.GetMyAccommodationsAsync(userId);
        }

        public async Task<AccommodationDto> InsertAccommodationAsync(AccommodationDto accommodationDto)
        {
            Accommodation accommodation = AccommodationDto.MapDtoToAccommodation(accommodationDto);

            await  _dbContext.InsertAccommodationAsync(accommodation);

            return accommodationDto;
        }

        public AccommodationDto UpdateAccommodation(AccommodationDto accommodationDto)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);

                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }

        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;
                Console.WriteLine(message);
                
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Error processing Kafka message: {ex.Message}");
            }
        
        }
    }
}

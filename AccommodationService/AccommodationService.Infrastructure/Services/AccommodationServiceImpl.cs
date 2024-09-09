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
using System.Text.Json;
using AccommodationService.Application.Interfaces.Kafka;

namespace AccommodationService.Infrastructure.Services
{
    public class AccommodationServiceImpl : IAccommodationService
    {
        private readonly AccommodationContext _dbContext;

        private readonly IProducer _producer;

        private readonly IConsumer<Ignore, string> _consumer;
        public AccommodationServiceImpl(AccommodationContext dbContext, IProducer producer)
        {
            _dbContext = dbContext;
            _producer = producer;
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:29092",
                GroupId = "Bookingg",
                AutoOffsetReset = AutoOffsetReset.Latest  
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _consumer.Subscribe("reservation_request");
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
            await Task.Run(async () =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);
                        var message = consumeResult.Message.Value;

                        var topic = consumeResult.Topic;
                        Console.WriteLine("sa topica " + topic);
                        Console.WriteLine($"Primljena poruka: {message}");
                        await ProcessReservationRequestMessage(message);


                    }
                }
                catch (OperationCanceledException)
                {
                    
                    Console.WriteLine("Operacija otkazana.");
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Greška: {ex.Message}");
                }
            });
        }

        public async Task ProcessReservationRequestMessage(string message)
        {
            ReservationRequestDto reservationRequestDto = JsonSerializer.Deserialize<ReservationRequestDto>(message);
            var accommodation = await _dbContext.GetAccommodationByIdAsync(reservationRequestDto.AccommodationId);
            if(accommodation!=null && reservationRequestDto.DateFrom >= accommodation.AvailableFrom && reservationRequestDto.DateTo <= accommodation.AvailableTo)
            {
                await _producer.ProduceAsync("reservation_success", message);
            }
            else
            {
                await _producer.ProduceAsync("reservation_failed", "message");
            }
           
        }
    }
}

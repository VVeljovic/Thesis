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
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AccommodationService.Infrastructure.Services
{
    public class AccommodationServiceImpl : IAccommodationService
    {
        private readonly AccommodationContext _dbContext;

        IRabbitMQProducer<ReservationRequestDto> _rabbitMQProducer;

        IRabbitMQProducer<string> _rabbitMQProducer1;

        public AccommodationServiceImpl(AccommodationContext dbContext, IRabbitMQProducer<ReservationRequestDto> rabbitMQProducer, IRabbitMQProducer<string>rabbitMQProducer1)
        {
            _dbContext = dbContext;
            _rabbitMQProducer = rabbitMQProducer;
            _rabbitMQProducer1 = rabbitMQProducer1;
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

        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsAsync(double longitude, double latitude, int pageSize, int pageNumber, string? address = null, DateOnly? checkIn = null, DateOnly? checkOut = null, int? numberofGuests = null)
        {
            return await _dbContext.GetAccommodationsAsync(longitude, latitude, pageSize, pageNumber, address, checkIn, checkOut, numberofGuests);
        }

        public async Task<IEnumerable<AccommodationDto>> GetMyAccommodationsAsync(string userId)
        {
            return await _dbContext.GetMyAccommodationsAsync(userId);
        }
        public async Task<Task> HandleMessageAsync<T>(T message, string queueName)
        {
            var messageJson = JsonSerializer.Serialize(message);

            Console.WriteLine($"Received message from queue: {queueName}");
            Console.WriteLine("Message content:");
            Console.WriteLine(messageJson);
            if (queueName == "transaction_request" && message is ReservationRequestDto reservationRequest)
            {
                 var accommodation = await GetAccommodationByIdAsync(reservationRequest.AccommodationId);
                 if(accommodation == null)
                 {
                    await _rabbitMQProducer1.PublishMessageAsync("Accommodation with this id does not exist.","transaction_failed");
                 }
                 else
                 {
                await _rabbitMQProducer.PublishMessageAsync(reservationRequest, "reservation_request");
                 }
            }
            return Task.CompletedTask;
        }
        public async Task<AccommodationDto> InsertAccommodationAsync(AccommodationDto accommodationDto)
        {
            Accommodation accommodation = AccommodationDto.MapDtoToAccommodation(accommodationDto);

            await _dbContext.InsertAccommodationAsync(accommodation);

            return accommodationDto;
        }
        public AccommodationDto UpdateAccommodation(AccommodationDto accommodationDto)
        {
            throw new NotImplementedException();
        }
    }
}

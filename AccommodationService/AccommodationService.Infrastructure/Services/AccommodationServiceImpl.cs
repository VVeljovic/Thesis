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
using System.Transactions;
using AccommodationService.Application.Dtos.ChoreographyDtos;

namespace AccommodationService.Infrastructure.Services
{
    public class AccommodationServiceImpl : IAccommodationService
    {
        private readonly AccommodationContext _dbContext;

        IRabbitMQProducer<TransactionRequestDto> _producerForTransactionRequest;

        IRabbitMQProducer<TransactionResponseDto> _producerForTransactionResponse;

        public AccommodationServiceImpl(AccommodationContext dbContext, IRabbitMQProducer<TransactionRequestDto> producerForTransactionRequest, IRabbitMQProducer<TransactionResponseDto> producerForTransactionResponse)
        {
            _dbContext = dbContext;
            _producerForTransactionRequest = producerForTransactionRequest;
            _producerForTransactionResponse = producerForTransactionResponse;
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
        public async Task HandleMessageAsync<T>(T message, string queueName)
        {

            if (queueName == "transaction_request")
            {
                await TransactionRequestAsync(message, "transaction_request");
                return;
            }
            else if (queueName == "reservation_failed")
            {
                await TransactionFailedAsync(message, "reservation_failed");
                return;
            }
            else
            {
                await TransactionSuccessAsync(message, "reservation_success");
                return;
            }
        }
        private async Task TransactionRequestAsync<T>(T message, string queueName)
        {
            var reservationRequest = message as TransactionRequestDto;
            {
                Console.WriteLine(reservationRequest.StripeUserDto.CreditCard.CreditCard);
                var accommodation = await GetAccommodationByIdAsync(reservationRequest.AccommodationId);
                if (accommodation == null)
                {
                    var failedMessage = new TransactionResponseDto()
                    {
                        Message = "\"Accommodation with this id does not exist.",
                        TransactionId = reservationRequest.TransactionId
                    };
                    await _producerForTransactionResponse.PublishMessageAsync(failedMessage, "transaction_failed");
                }
                else
                {
                    bool isDateWithinRange = reservationRequest.DateFrom >= accommodation.AvailableFrom &&
                                             reservationRequest.DateTo <= accommodation.AvailableTo;

                    if (!isDateWithinRange)
                    {
                        var failedMessage = new TransactionResponseDto()
                        {
                            Message = "The requested dates are out of the accommodation's availability range.",
                            TransactionId = reservationRequest.TransactionId
                        };
                        await _producerForTransactionResponse.PublishMessageAsync(failedMessage, "transaction_failed");
                    }
                    else
                    {
                        await _producerForTransactionRequest.PublishMessageAsync(reservationRequest, "reservation_request");
                    }
                }
            }
        }
        private async Task TransactionSuccessAsync<T>(T message, string queueName)
        {
            var transactionResponse = message as TransactionResponseDto;
            await _producerForTransactionResponse.PublishMessageAsync(transactionResponse, "transaction_success");
            
        }

        private async Task TransactionFailedAsync<T>(T message, string queueName)
        {
            var transactionResponse = message as TransactionResponseDto;
            await _producerForTransactionResponse.PublishMessageAsync(transactionResponse, "transaction_failed");
        }
        public async Task<AccommodationDto> InsertAccommodationAsync(AccommodationDto accommodationDto)
        {
            Accommodation accommodation = AccommodationDto.MapDtoToAccommodation(accommodationDto);

            await _dbContext.InsertAccommodationAsync(accommodation);

            return accommodationDto;
        }

        public async Task<AccommodationDto> UpdateAccommodationAsync(AccommodationDto accommodationDto)
        {
            Accommodation accommodation = AccommodationDto.MapDtoToAccommodation(accommodationDto);
            accommodation.Id = new MongoDB.Bson.ObjectId(accommodationDto.Id);
            await _dbContext.UpdateAccommodationAsync(accommodation);
            return accommodationDto;
        }
    }
}

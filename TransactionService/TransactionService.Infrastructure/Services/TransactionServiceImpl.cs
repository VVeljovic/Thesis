using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Interfaces;
using TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand;
using TransactionService.Infrastructure.MongoDb;
using System.Text.Json;
using System.ComponentModel;
using System.Threading;
using TransactionService.Domain.Entities;
using TransactionService.Application.Dtos;

namespace TransactionService.Infrastructure.Services
{
    public class TransactionServiceImpl : ITransactionService
    {
        private readonly TransactionDbContext _transactionDbContext;

        IRabbitMQProducer<TransactionRequestDto> _rabbitMQPublisher;
        public TransactionServiceImpl(TransactionDbContext transactionDbContext, IRabbitMQProducer<TransactionRequestDto> rabbitMQPublisher)
        {
            _transactionDbContext = transactionDbContext;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        public async Task CreateTransaction(CreateTransactionCommand createTransactionCommand)
        {
            var transaction = CreateTransactionCommand.MapToTransaction(createTransactionCommand);
            await _transactionDbContext.CreateTransactionAsync(transaction);
            var transactionRequest = TransactionRequestDto.MapToTransactionRequestDto(transaction);
            transactionRequest.StripeUserDto = createTransactionCommand.StripeUserDto;
            await _rabbitMQPublisher.PublishMessageAsync(transactionRequest, "transaction_request");
        }
        public async Task<string> HandleMessageAsync<T>(T message, string queueName)
        {
            if (queueName == "transaction_failed")
            {
                return await TransactionFailedAsync(message, "transaction_failed");
                
            }
            else
            {
                return await TransactionSuccessAsync(message, "transaction_success");
               
            }
        }
        private async Task<string> TransactionSuccessAsync<T>(T message, string queueName)
        {
            var transactionSuccessMessageDto = message as TransactionResponseDto;
            Console.WriteLine(transactionSuccessMessageDto.Message);
            await _transactionDbContext.UpdateTransactionStatusAsync(transactionSuccessMessageDto.TransactionId, transactionSuccessMessageDto.TransactionStatus);
            return transactionSuccessMessageDto.Message;
        }

        private async Task<string> TransactionFailedAsync<T>(T message, string queueName)
        {
            var transactionFailedMessageDto = message as TransactionResponseDto;
            Console.WriteLine(transactionFailedMessageDto.Message);
            await _transactionDbContext.UpdateTransactionStatusAsync(transactionFailedMessageDto.TransactionId, transactionFailedMessageDto.TransactionStatus);
            return transactionFailedMessageDto.Message;
        }
    }
}

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

namespace TransactionService.Infrastructure.Services
{
    public class TransactionServiceImpl :ITransactionService
    {
        private readonly TransactionDbContext _transactionDbContext;

        IRabbitMQProducer<CreateTransactionCommand> _rabbitMQPublisher;
        public TransactionServiceImpl(TransactionDbContext transactionDbContext, IRabbitMQProducer<CreateTransactionCommand> rabbitMQPublisher)
        {  
            _transactionDbContext = transactionDbContext;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        public async Task CreateTransaction(CreateTransactionCommand createTransactionCommand)
        {
            var transaction = CreateTransactionCommand.MapToTransaction(createTransactionCommand);
            await _transactionDbContext.CreateTransactionAsync(transaction);
            await _rabbitMQPublisher.PublishMessageAsync(createTransactionCommand, RabbitMQQueues.ReservationRequest);
        }

        public Task HandleMessageAsync<T>(T message, string queueName)
        {
            var messageJson = JsonSerializer.Serialize(message);
            Console.WriteLine($"Received message from queue: {queueName}");
            Console.WriteLine("Message content:");
            Console.WriteLine(messageJson);

            return Task.CompletedTask;
        }
    }
}

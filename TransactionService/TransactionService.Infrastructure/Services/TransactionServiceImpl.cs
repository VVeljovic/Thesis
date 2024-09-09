using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Interfaces;
using Confluent.Kafka;
using TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand;
using TransactionService.Infrastructure.MongoDb;
using System.Text.Json;
using System.ComponentModel;
using System.Threading;

namespace TransactionService.Infrastructure.Services
{
    public class TransactionServiceImpl :ITransactionService
    {

        private readonly IEnumerable<string> topics = ["reservation_success","reservation_failed"];

        private readonly IProducer _producer;

        private readonly IConsumer<Ignore, string> _consumer;

        private readonly TransactionDbContext _transactionDbContext;
        public TransactionServiceImpl(TransactionDbContext transactionDbContext, IProducer producer)
        {
            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:29092",
                GroupId = "Bookingg",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
            };
            _producer = producer;
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _transactionDbContext = transactionDbContext;
            _consumer.Subscribe(topics);
        }
        public async Task CreateTransaction(CreateTransactionCommand createTransactionCommand)
        {
            var transaction = CreateTransactionCommand.MapToTransaction(createTransactionCommand);
            await _transactionDbContext.CreateTransactionAsync(transaction);
            string jsonTransaction = JsonSerializer.Serialize<CreateTransactionCommand>(createTransactionCommand);
            await _producer.ProduceAsync("reservation_request", jsonTransaction);
        }
         public async Task ReadMessageFromTopic(CancellationToken stoppingToken)
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
    }
}

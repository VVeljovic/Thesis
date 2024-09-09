using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces.Kafka;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccommodationService.Infrastructure.Services.Kafka
{
    public class ProducerImpl : IProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ProducerImpl(IProducer<Null, string> producer)
        {
            _producer = producer;
        }
        public async Task ProduceAsync(string topic, string message)
        {
            var kafkaMessage = new Message<Null, string> { Value = message };
            await _producer.ProduceAsync(topic, kafkaMessage);
            Console.WriteLine(kafkaMessage);
        }
    }
}

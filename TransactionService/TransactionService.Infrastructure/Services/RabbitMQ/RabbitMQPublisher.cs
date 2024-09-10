

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQPublisher<T> : IRabbitMQPublisher<T>
{
    private readonly IModel _channel;
    public RabbitMQPublisher(IModel channel)
    {
        _channel = channel;   
    }
    public async Task PublishMessageAsync(T message, string queueName)
    {
       _channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var messageBody = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageBody);
        _channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        await Task.CompletedTask;
    }
}
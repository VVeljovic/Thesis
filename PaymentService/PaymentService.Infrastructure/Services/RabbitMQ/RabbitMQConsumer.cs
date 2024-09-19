
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQConsumer<T> : IRabbitMQConsumer<T>
{
    private readonly IModel _channel;
    private readonly string[] _queueNames;

    public RabbitMQConsumer(IModel channel, string[] queueNames)
    {
        _channel = channel;
        _queueNames = queueNames;

        foreach (var queueName in _queueNames)
        {
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }
    }

    public void StartConsuming(Func<T, string, Task> handleMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(messageJson);

            await handleMessage(message, ea.RoutingKey);
        };

        foreach (var queueName in _queueNames)
        {
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
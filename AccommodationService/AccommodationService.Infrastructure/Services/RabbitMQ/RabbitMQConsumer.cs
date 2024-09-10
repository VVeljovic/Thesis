
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQConsumer<T> : IRabbitMQConsumer<T>
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName = "reservation_request";

    public RabbitMQConsumer(IConnection connection, IModel channel)
    {
        _connection = connection;
        _channel = channel;

        _channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }
    public void StartConsuming(Func<T, Task> handleMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(messageJson);

            await handleMessage(message);
        };

        _channel.BasicConsume(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);
    }
}
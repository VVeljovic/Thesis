public interface IRabbitMQProducer<T>
{
    Task PublishMessageAsync(T message, string queueName);
}
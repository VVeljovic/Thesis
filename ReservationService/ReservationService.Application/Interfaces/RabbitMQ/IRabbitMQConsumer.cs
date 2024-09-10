public interface IRabbitMQConsumer<T>
{
    public void StartConsuming(Func<T, string, Task> handleMessage);
}
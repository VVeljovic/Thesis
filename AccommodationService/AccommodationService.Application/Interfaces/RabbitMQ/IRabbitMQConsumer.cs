public interface IRabbitMQConsumer<T>
{
    public void StartConsuming(Func<T, Task> handleMessage);
}
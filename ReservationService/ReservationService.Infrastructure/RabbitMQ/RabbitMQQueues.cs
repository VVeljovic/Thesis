public class RabbitMQQueues
{
    public string[] TransactionRequestQueues { get; set; } = { "reservation_request" };
    public string[] TransactionResponseQueues { get; set; } = { "payment_success", "payment_failed" };
}
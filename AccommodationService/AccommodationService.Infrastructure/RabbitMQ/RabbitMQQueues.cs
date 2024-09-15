public class RabbitMQQueues
{
    public string[] TransactionRequestQueues { get; set; } = { "transaction_request" };
    public string[] TransactionResponseQueues { get; set; } = { "reservation_success", "reservation_failed" };
}
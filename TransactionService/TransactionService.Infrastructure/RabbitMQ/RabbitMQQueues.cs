public class RabbitMQQueues
{
    public string[] TransactionResponseQueues { get; set; } = { "transaction_success", "transaction_failed" };
}
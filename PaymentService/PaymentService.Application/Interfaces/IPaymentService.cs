using Stripe;

public interface IPaymentService
{
    Task<StripeCustomer> AddStripeCustomerAsync(StripeUserDto customer);

    Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment);

    Task<Customer> GetCustomerByEmailAsync(string email);

    public Task HandleMessageAsync<T>(T message, string queueName);

}
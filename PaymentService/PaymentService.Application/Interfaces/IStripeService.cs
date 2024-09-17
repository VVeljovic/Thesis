using Stripe;

public interface IStripeService
{
    Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
    Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
    Task<Customer> GetCustomerByEmailAsync(string email, CancellationToken ct);
}
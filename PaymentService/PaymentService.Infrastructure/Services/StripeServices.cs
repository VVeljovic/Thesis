using Stripe;
public class StripeService : IStripeService
{
    private readonly ChargeService _chargeService;
    private readonly CustomerService _customerService;
    private readonly TokenService _tokenService;
    private const string SecretKey = "sk_test_51NDomrIqJ8VSwXB7ICwnmJKXGe64KVpmPPme9Ea4KaKdNCwdFoDmIAT8TARwzNvntK0G4uockFb4ziGIyrlE3Sgy00aeGgD9Ro";

    public StripeService(
        ChargeService chargeService,
        CustomerService customerService,
        TokenService tokenService)
    {
        _chargeService = chargeService;
        _customerService = customerService;
        _tokenService = tokenService;
         StripeConfiguration.ApiKey = SecretKey;
    }

    public async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct)
    {
        TokenCreateOptions tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Name = customer.Name,
                Number = customer.CreditCard.CardNumber,
                ExpYear = customer.CreditCard.ExpirationYear,
                ExpMonth = customer.CreditCard.ExpirationMonth,
                Cvc = customer.CreditCard.Cvc
            }
        };
        Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, ct);
        CustomerCreateOptions customerOptions = new CustomerCreateOptions
        {
            Name = customer.Name,
            Email = customer.Email,
            Source = stripeToken.Id
        };
        Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, ct);
        return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
    }

    public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct)
    {
        ChargeCreateOptions paymentOptions = new ChargeCreateOptions
        {
            Customer = payment.CustomerId,
            ReceiptEmail = payment.ReceiptEmail,
            Description = payment.Description,
            Currency = payment.Currency,
            Amount = payment.Amount
        };
        var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, ct);
        return new StripePayment(
          createdPayment.CustomerId,
          createdPayment.ReceiptEmail,
          createdPayment.Description,
          createdPayment.Currency,
          createdPayment.Amount,
          createdPayment.Id);
    }
}

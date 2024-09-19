using System.Text.Json;
using AccommodationService.Application.Dtos.ChoreographyDtos;
using Stripe;
public class PaymentServiceImpl : IPaymentService
{
    private readonly ChargeService _chargeService;
    private readonly CustomerService _customerService;
    private readonly TokenService _tokenService;

    IRabbitMQProducer<TransactionResponseDto> _producerForTransactionResponse;
    private const string SecretKey = "sk_test_51NDomrIqJ8VSwXB7ICwnmJKXGe64KVpmPPme9Ea4KaKdNCwdFoDmIAT8TARwzNvntK0G4uockFb4ziGIyrlE3Sgy00aeGgD9Ro";

    public PaymentServiceImpl(
        ChargeService chargeService,
        CustomerService customerService,
        TokenService tokenService,
        IRabbitMQProducer<TransactionResponseDto> producerForTransactionResponse)
    {
        _chargeService = chargeService;
        _customerService = customerService;
        _tokenService = tokenService;
        StripeConfiguration.ApiKey = SecretKey;
        _producerForTransactionResponse = producerForTransactionResponse;
    }

    public async Task<StripeCustomer> AddStripeCustomerAsync(StripeUserDto customer)
    {
        TokenCreateOptions tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Name = customer.Name,
                Number = customer.CreditCard.CreditCard,
                ExpYear = customer.CreditCard.ExpirationYear,
                ExpMonth = customer.CreditCard.ExpirationMonth,
                Cvc = customer.CreditCard.Cvc
            }
        };
        Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null);
        CustomerCreateOptions customerOptions = new CustomerCreateOptions
        {
            Name = customer.Name,
            Email = customer.Email,
            Source = stripeToken.Id
        };
        Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null);
        return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
    }

    public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment)
    {
        ChargeCreateOptions paymentOptions = new ChargeCreateOptions
        {
            Customer = payment.CustomerId,
            Description = payment.Description,
            Currency = payment.Currency,
            Amount = payment.Amount
        };
        var createdPayment = await _chargeService.CreateAsync(paymentOptions, null);
        return new StripePayment(
          createdPayment.CustomerId,
          createdPayment.ReceiptEmail,
          createdPayment.Description,
          createdPayment.Currency,
          createdPayment.Amount,
          createdPayment.Id);
    }
    public async Task<Customer> GetCustomerByEmailAsync(string email)
    {
        var options = new CustomerListOptions
        {
            Email = email,
            Limit = 1
        };

        var customers = await _customerService.ListAsync(options, null);

        return customers.Data.FirstOrDefault();
    }

    public async Task HandleMessageAsync<T>(T message, string queueName)
    {
        var requestMessage = message as TransactionRequestDto;
        Console.WriteLine(requestMessage.StripeUserDto.CreditCard.CreditCard);
        var stripeCustomer = await GetCustomerByEmailAsync(requestMessage.StripeUserDto.Email);
        var messageResponse = new TransactionResponseDto()
        {
            Message = "",
            TransactionId = requestMessage.TransactionId,
            TransactionStatus = "",
            ReservationId = requestMessage.ReservationId
        };
        try
        {
            if (stripeCustomer == null)
            {
                await AddNewCustomerAsync(requestMessage.StripeUserDto, requestMessage);
            }
            else
            {
                var stripePayment = new AddStripePayment(stripeCustomer.Id, "Booking accommodation", "USD", (long)requestMessage.TotalAmount);

                var result = await AddStripePaymentAsync(stripePayment);
                if (!string.IsNullOrEmpty(result.PaymentId))
                {
                    messageResponse.TransactionStatus = "Success";
                    messageResponse.Message = "Payment successful!";
                    await _producerForTransactionResponse.PublishMessageAsync(messageResponse, "payment_success");
                }
            }
        }
        catch (StripeException ex)
        {
            messageResponse.TransactionStatus = "Failed";
            messageResponse.Message = $"{ex.Message}";
            await _producerForTransactionResponse.PublishMessageAsync(messageResponse, "payment_failed");
        }
    }

    private async Task AddNewCustomerAsync(StripeUserDto customer, TransactionRequestDto transactionRequestDto)
    {
        var result = await AddStripeCustomerAsync(customer);
        var newPayment = new AddStripePayment(result.CustomerId, "Booking Accommodation", "USD", (long)transactionRequestDto.TotalAmount);
         var messageResponse = new TransactionResponseDto()
        {
            Message = "",
            TransactionId = transactionRequestDto.TransactionId,
            TransactionStatus = "",
            ReservationId = transactionRequestDto.ReservationId
        };
        try
        {
            var paymentResult = await AddStripePaymentAsync(newPayment);
            if (!string.IsNullOrEmpty(paymentResult.PaymentId))
            {
                messageResponse.TransactionStatus = "Success";
                messageResponse.Message = "Payment successful!";
                await _producerForTransactionResponse.PublishMessageAsync(messageResponse, "payment_success");
            }
        }
        catch (StripeException ex)
        {
            messageResponse.TransactionStatus = "Failed";
            messageResponse.Message = $"{ex.Message}";
            await _producerForTransactionResponse.PublishMessageAsync(messageResponse, "payment_failed");
        }
    }
}

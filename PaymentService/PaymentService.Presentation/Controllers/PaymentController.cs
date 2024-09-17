using Microsoft.AspNetCore.Mvc;
using Stripe;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public PaymentController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("add-customer")]
    public async Task<IActionResult> AddStripeCustomer([FromBody] AddStripeCustomer request, CancellationToken ct)
    {
        if (request == null)
        {
            return BadRequest("Invalid customer data.");
        }

        try
        {
            var result = await _stripeService.AddStripeCustomerAsync(request, ct);
            return Ok(result);
        }
        catch (StripeException ex)
        {
            return StatusCode(500, $"Stripe error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }

    [HttpPost("add-payment")]
    public async Task<IActionResult> AddStripePayment([FromBody] AddStripePayment request, CancellationToken ct)
    {
        if (request == null)
        {
            return BadRequest("Invalid payment data.");
        }

        try
        {
            var result = await _stripeService.AddStripePaymentAsync(request, ct);
            return Ok(result);
        }
        catch (StripeException ex)
        {
            return StatusCode(500, $"Stripe error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }
    [HttpGet("email")]
    public async Task<IActionResult> GetCustomerByEmailAsync([FromQuery] string email, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }

        var customer = await _stripeService.GetCustomerByEmailAsync(email, ct);

        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        return Ok(new
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        });
    }


}
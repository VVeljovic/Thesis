using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    public readonly IMediator _mediator;

    public ReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNewReserationAsync([FromBody]CreateReservationCommand createReservationCommand)
    {
        await _mediator.Send(createReservationCommand);
        return Ok();
    }
}
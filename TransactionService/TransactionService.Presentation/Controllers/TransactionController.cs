using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Dtos;
using TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand;
using TransactionService.Application.Interfaces;

namespace TransactionService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTransacationAsync([FromBody]CreateTransactionCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
       
    }
}

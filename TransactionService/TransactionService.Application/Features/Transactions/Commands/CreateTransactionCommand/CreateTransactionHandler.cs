using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Interfaces;

namespace TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, CreateTransactionCommand>
    {
        public readonly ITransactionService _transactionService;

        public CreateTransactionHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        Task<CreateTransactionCommand> IRequestHandler<CreateTransactionCommand, CreateTransactionCommand>.Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            _transactionService.CreateTransaction(request);
            return Task.FromResult(request);
        }
    }
}

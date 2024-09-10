using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand;

namespace TransactionService.Application.Interfaces
{
    public interface ITransactionService
    {
        public Task CreateTransaction(CreateTransactionCommand createTransactionCommand);
        public Task HandleMessageAsync<T>(T message, string queueName);
    }
}

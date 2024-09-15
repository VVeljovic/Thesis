using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Dtos;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand
{
    public class CreateTransactionCommand : IRequest<CreateTransactionCommand>
    {

        public DateTime Date { get; set; }

        public double TotalAmount { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string AccommodationId { get; set; }

        public string Type { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public static Transaction MapToTransaction(CreateTransactionCommand createTransactionCommand)
        {
            return new Transaction()
            {
                Date = createTransactionCommand.Date,
                TotalAmount = createTransactionCommand.TotalAmount,
                Status = createTransactionCommand.Status,
                UserId = createTransactionCommand.UserId,
                AccommodationId = createTransactionCommand.AccommodationId,
                Type = createTransactionCommand.Type,
                DateFrom=createTransactionCommand.DateFrom,
                DateTo=createTransactionCommand.DateTo         
            };
        }
    }
}

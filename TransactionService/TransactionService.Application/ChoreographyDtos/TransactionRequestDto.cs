using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Application.Features.Transactions.Commands.CreateTransactionCommand;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Dtos
{
    public class TransactionRequestDto
    {
        public string? TransactionId { get; set; }

        public DateTime Date { get; set; }

        public double TotalAmount { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string AccommodationId { get; set; }

        public string Type { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string ReservationId { get; set; } = "";


        public static TransactionRequestDto MapToTransactionRequestDto(Transaction transaction)
        {
            return new TransactionRequestDto()
            {
                TransactionId = transaction.Id.ToString(),
                Date = transaction.Date,
                TotalAmount = transaction.TotalAmount,
                Status = transaction.Status,
                UserId = transaction.UserId,
                AccommodationId = transaction.AccommodationId,
                Type = transaction.Type,
                DateFrom = transaction.DateFrom,
                DateTo = transaction.DateTo,
                ReservationId = ""
            };
        }
    }
}

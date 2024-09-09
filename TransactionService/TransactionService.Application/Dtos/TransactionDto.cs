using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Dtos
{
    public class TransactionDto
    {
        public DateTime Date { get; set; }

        public double TotalAmount { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string Type { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string AccommodationId { get; set; }

        public static Transaction MapTransactionDtoToTransaction(TransactionDto transactionDto)
        {
            return new Transaction()
            {
                Date = transactionDto.Date,
                TotalAmount = transactionDto.TotalAmount,
                Status = transactionDto.Status,
                UserId = transactionDto.UserId,
                AccommodationId = transactionDto.AccommodationId,
                Type = transactionDto.Type
            };
        }
    }
}

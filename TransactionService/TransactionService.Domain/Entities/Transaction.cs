using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionService.Domain.Entities
{
    public class Transaction
    {
        public ObjectId Id { get; set; }

        public DateTime Date { get; set; }

        public double  TotalAmount { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string UserId { get; set; }

        public string AccommodationId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

    }
}

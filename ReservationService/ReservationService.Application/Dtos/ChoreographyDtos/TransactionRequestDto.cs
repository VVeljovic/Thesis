﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AccommodationService.Application.Dtos
{
    public class TransactionRequestDto
    {
        public string TransactionId { get; set; }

        public DateTime Date { get; set; }

        public double TotalAmount { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string AccommodationId { get; set; }

        public string Type { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string ReservationId { get; set; }

        public StripeUserDto StripeUserDto { get; set; }


    }
}

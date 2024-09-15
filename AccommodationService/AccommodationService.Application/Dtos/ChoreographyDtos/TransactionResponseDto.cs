using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Application.Dtos.ChoreographyDtos
{
    public class TransactionResponseDto
    {
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public string TransactionStatus { get; set; }

    }
}

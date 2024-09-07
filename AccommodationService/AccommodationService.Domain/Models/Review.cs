using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Domain.Models
{
    public class Review
    {
        public ObjectId Id { get; set; }

        public string UserId { get; set; } 

        public string AccommodationId { get; set; } 

        public int Rating { get; set; } 

        public string Comment { get; set; } 

        public DateTime DateCreated { get; set; } 

    }
}

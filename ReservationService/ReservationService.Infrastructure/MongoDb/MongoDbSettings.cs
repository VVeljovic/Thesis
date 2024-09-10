using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Infrastructure.MongoDb
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = "mongodb://root:example@localhost:27017";
        public string DatabaseName { get; set; } = "ReservationsDb";
    }
}

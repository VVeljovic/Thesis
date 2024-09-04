using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.DBContext
{
    public class ScyllaDbSettings
    {
        public string[] ContactPoints { get; set; }

        public int Port { get; set; }

        public string Keyspace { get; set; }
    }
}

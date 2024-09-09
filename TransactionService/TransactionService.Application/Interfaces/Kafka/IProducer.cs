using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionService.Application.Interfaces
{
    public interface IProducer
    {
        public Task ProduceAsync(string topic, string message);
    }
}

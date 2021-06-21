using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDataService.Models
{
    public class RabbitMQConfig
    {
        public String QueueName { get; set; }
        public String URI { get; set; }
    }
}

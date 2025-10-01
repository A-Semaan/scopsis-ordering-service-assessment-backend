using Microsoft.Extensions.Logging;
using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Models
{
    public class ApplicationLogModel
    {
        public long LogID { get; set; }

        public DateTime LogDate { get; set; }

        public APPLICATION_LOG_EVENT Event { get; set; }

        public string? EventInfo { get; set; }

        public Customer? Customer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Models
{
    public class ItemModel
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public bool Deleted { get; set; }

        public DateTime DeletedOn { get; set; }
    }
}

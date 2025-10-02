using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Models
{
    public class OrderItemModel
    {
        public ItemModel Item { get; set; }
        public int Count { get; set; }
    }
}

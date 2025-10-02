using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.Entities
{
    public class OrderItem
    {
        [Column("order_id")]
        public long OrderID { get; set; }
        public Order Order { get; set; }

        [Column("item_id")]
        public long ItemID { get; set; }
        public Item Item { get; set; }

        [Column("Count")]
        public int Count { get; set; }
    }
}

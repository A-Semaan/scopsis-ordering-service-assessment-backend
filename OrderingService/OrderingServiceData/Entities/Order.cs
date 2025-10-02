using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.Entities
{
    public class Order
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("customer_id")]
        public long? CustomerID { get; set; }

        [ForeignKey(nameof(CustomerID))]
        public Customer? Customer { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("total_amount")]
        public double TotalAmount { get; set; }

        [Column("items")]
        public List<OrderItem> Items { get; set; }
    }
}

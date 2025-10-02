using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Models
{
    public class OrderModel
    {
        public long ID { get; set; }

        public CustomerModel Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public double TotalAmount { get; set; }

        public List<OrderItemModel> Items { get; set; }
    }
}

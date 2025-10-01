using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess.Abstractions
{
    public interface IOrderDataAccess
    {
        public long InsertOrder(Order order);

        public Order? GetOrder(long orderID);

        public List<Order> GetOrdersByCustomer(long customerID);
    }
}

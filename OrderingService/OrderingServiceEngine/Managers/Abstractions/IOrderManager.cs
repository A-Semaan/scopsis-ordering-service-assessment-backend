using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers.Abstractions
{
    public interface IOrderManager
    {
        public long InsertOrder(Dictionary<long, int> itemIDs, long customerID);
        public OrderModel? GetOrder(long orderID);
        public List<OrderModel> GetOrdersByCustomer(long customerID);
    }
}

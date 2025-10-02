using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess
{
    public class OrderDataAccess : IOrderDataAccess
    {
        private readonly OrderingServiceDbContext _dbContext;

        public OrderDataAccess(OrderingServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order? GetOrder(long orderID)
        {
            Order? order = _dbContext.Orders.Include(o=>o.Items).ThenInclude(oi => oi.Item).Where(o=>o.ID== orderID).FirstOrDefault();

            return order;
        }

        public List<Order> GetOrdersByCustomer(long customerID)
        {
            List<Order> list = _dbContext.Orders.Include(o => o.Items).ThenInclude(oi => oi.Item).Where(o => o.Customer.ID == customerID).ToList();

            return list;
        }

        public long InsertOrder(Order order)
        {
            _dbContext.Orders.Add(order);

            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to insert order");
            }

            return order.ID;
        }
    }
}

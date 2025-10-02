using AutoMapper;
using OrderingServiceData.DataAccess;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderDataAccess _orderDataAccess;
        private readonly ICustomerDataAccess _customerDataAccess;
        private readonly IItemDataAccess _itemDataAccess;

        private readonly IApplicationLogManager _applicationLogManager;
        private readonly IMapper _mapper;

        public OrderManager(IOrderDataAccess orderDataAccess, IApplicationLogManager applicationLogManager, IMapper mapper, ICustomerDataAccess customerDataAccess, IItemDataAccess itemDataAccess)
        {
            _orderDataAccess = orderDataAccess;
            _applicationLogManager = applicationLogManager;
            _mapper = mapper;
            _customerDataAccess = customerDataAccess;
            _itemDataAccess = itemDataAccess;
        }

        public long InsertOrder(Dictionary<long, int> itemIDs, long customerID)
        {
            // Ensure the customer ID in the token matches the customer ID in the order
            Customer customer = _customerDataAccess.GetCustomer(customerID);

            List<Item> items = _itemDataAccess.GetItems(itemIDs.Keys.ToList());

            Order order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.UtcNow,
                TotalAmount = items.Select(item => item.Price * itemIDs[item.ID]).Sum(),
                Items = items.Select(item => new OrderItem
                {
                    Item = item,
                    Count = itemIDs[item.ID]
                }).ToList()
            };

            if (order.TotalAmount != order.Items.Select(i => i.Item.Price * i.Count).Sum())
            {
                throw new ArgumentException("Total amount does not match sum of item prices.");
            }

            long orderID = _orderDataAccess.InsertOrder(_mapper.Map<OrderingServiceData.Entities.Order>(order));

            if (orderID > 0)
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.ORDER_CREATED, orderID.ToString(), order.Customer.ID);
                return orderID;
            }

            return -1;
        }

        public OrderModel? GetOrder(long orderID)
        {
            return _mapper.Map<OrderModel?>(_orderDataAccess.GetOrder(orderID));
        }

        public List<OrderModel> GetOrdersByCustomer(long customerID)
        {
            return _mapper.Map<List<OrderModel>>(_orderDataAccess.GetOrdersByCustomer(customerID));
        }
    }
}

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

        private readonly IApplicationLogManager _applicationLogManager;
        private readonly IMapper _mapper;

        public OrderManager(IOrderDataAccess orderDataAccess, IApplicationLogManager applicationLogManager, IMapper mapper)
        {
            _orderDataAccess = orderDataAccess;
            _applicationLogManager = applicationLogManager;
            _mapper = mapper;
        }

        public long InsertOrder(OrderModel order)
        {
            if (order.Customer == null || order.Items == null || order.Items.Count == 0)
            {
                throw new ArgumentException("Order must have a customer and at least one item.");
            }

            if (order.TotalAmount <= 0)
            {
                throw new ArgumentException("Total amount must be greater than zero.");
            }

            if (order.TotalAmount != order.Items.Select(i => i.Price).Sum())
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

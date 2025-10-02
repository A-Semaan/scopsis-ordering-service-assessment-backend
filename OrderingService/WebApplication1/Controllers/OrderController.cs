using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Managers;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using OrderingServiceWeb.DTOs;
using OrderingServiceWeb.Helpers.Abstractions;

namespace OrderingServiceWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        private readonly IItemManager _itemManager;
        private readonly ISecurityHelper _securityHelper;

        public OrderController(IOrderManager orderManager, IItemManager itemManager, ISecurityHelper securityHelper)
        {
            _orderManager = orderManager;
            _itemManager = itemManager;
            _securityHelper = securityHelper;
        }

        [HttpGet("GetOrder")]
        public ActionResult<OrderModel> GetOrder(long orderID)
        {
            // Validate input
            if (orderID <= 0)
            {
                return BadRequest("Invalid order ID");
            }

            // Get Order
            OrderModel? orderModel = _orderManager.GetOrder(orderID);

            if (orderModel == null)
            {
                return NotFound("Order not found");
            }
            return orderModel;
        }

        [HttpGet("GetOrders")]
        public ActionResult<List<OrderModel>> GetOrdersByCustomer()
        {
            long customerID = _securityHelper.GetCustomerID();

            // Get Orders
            List<OrderModel> orders = _orderManager.GetOrdersByCustomer(customerID);
            return orders;
        }

        [HttpGet("GetOrdersByCustomer")]
        public ActionResult<List<OrderModel>> GetOrdersByCustomer(long customerID)
        {
            // Validate input
            if (customerID <= 0)
            {
                return BadRequest("Invalid customer ID");
            }

            // Get Orders
            List<OrderModel> orders = _orderManager.GetOrdersByCustomer(customerID);
            return orders;
        }

        [HttpPost("InsertOrder")]
        public ActionResult<long> InsertOrder(OrderInsertDto order)
        {
            // Validate input
            if (order == null || order.ItemIDs == null)
            {
                return BadRequest("Invalid order data");
            }

            long customerID = _securityHelper.GetCustomerID();

            // Insert Order
            return _orderManager.InsertOrder(order.ItemIDs, customerID);
        }
    }
}

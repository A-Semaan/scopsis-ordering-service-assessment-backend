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
            // Ensure the customer ID in the token matches the customer ID in the order
            CustomerModel currentCustomer = _securityHelper.GetCustomer();
            if (currentCustomer == null)
            {
                return Unauthorized("Invalid Customer");
            }

            List<ItemModel> items = _itemManager.GetItems(order.ItemIDs);

            OrderModel orderModel = new OrderModel
            {
                Customer = currentCustomer,
                OrderDate = DateTime.UtcNow,
                TotalAmount = items.Select(item => item.Price).Sum(),
                Items = items
            };

            // Insert Order
            return _orderManager.InsertOrder(orderModel);
        }
    }
}

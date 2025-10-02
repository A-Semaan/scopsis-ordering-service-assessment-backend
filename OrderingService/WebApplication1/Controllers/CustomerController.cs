using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using OrderingServiceWeb.DTOs;
using OrderingServiceWeb.Helpers;
using OrderingServiceWeb.Helpers.Abstractions;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _customerManager;
        private readonly ISecurityHelper _securityHelper;

        public CustomerController(ICustomerManager customerManager, ISecurityHelper securityHelper)
        {
            _customerManager = customerManager;
            _securityHelper = securityHelper;
        }

        [HttpGet("GetCustomer")]
        public ActionResult<CustomerModel> GetCustomer(long customerID)
        {
            if (customerID <= 0)
            {
                return BadRequest("customer ID is required.");
            }

            // get customer
            CustomerModel temp = _customerManager.GetCustomer(customerID);

            if (temp == null)
            {
                return NotFound("Customer not found");
            }
            return temp;
        }

        [HttpGet("GetAllCustomers")]
        public ActionResult<List<CustomerModel>> GetAllCustomers()
        {
            // get all customer
            List<CustomerModel> temp = _customerManager.GetAllCustomers();

            if (temp == null)
            {
                return NotFound("Customer not found");
            }
            return temp;
        }

        [AllowAnonymous]
        [HttpGet("EmailExists")]
        public ActionResult<bool> EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("email is required.");
            }

            // check if email exists
            return _customerManager.CustomerEmailExists(email);
        }

        [AllowAnonymous]
        [HttpPost("InsertCustomer")]
        public IActionResult InsertCustomer(CustomerInsertDto request)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Name, Email, and Password are required.");
            }

            // Create customer
            long customerId = _customerManager.InsertCustomer(request.Name, request.Email, request.Password);

            CustomerModel temp = _customerManager.GetCustomer(customerId)!;
            string token = _securityHelper.GenerateToken(temp);

            return Ok(new { Token = token });
        }

        [HttpPost("UpdateCustomer")]
        public ActionResult<bool> UpdateCustomer(CustomerUpdateDto request)
        {
            if (request.CustomerID <= 0)
            {
                return BadRequest("CustomerID is required.");
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(request.Name) && string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("At least one of the Name, Email, or Password is required.");
            }

            // update customer
            return _customerManager.UpdateCustomer(request.CustomerID, request.Name, request.Email, request.Password);
        }

    }
}

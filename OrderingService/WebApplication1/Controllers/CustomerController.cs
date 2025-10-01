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
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _customerManager;
        private readonly ISecurityHelper _securityHelper;

        public CustomerController(ICustomerManager customerManager, ISecurityHelper securityHelper)
        {
            _customerManager = customerManager;
            _securityHelper = securityHelper;
        }

        [AllowAnonymous]
        [HttpPost("Insert")]
        public ActionResult<string> Insert([FromBody] CustomerInsertDto request)
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

            return token;
        }

        [HttpPost("Update")]
        public ActionResult<bool> Update([FromBody] CustomerUpdateDto request)
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

            // Create customer
            return _customerManager.UpdateCustomer(request.CustomerID, request.Name, request.Email, request.Password);
        }

        [AllowAnonymous]
        [HttpGet("EmailExists")]
        public ActionResult<bool> EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("email is required.");
            }

            // Sanitize inputs
            // Create customer
            return _customerManager.CustomerEmailExists(email);
        }

    }
}

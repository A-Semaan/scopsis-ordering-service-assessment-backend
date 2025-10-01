using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using OrderingServiceWeb.DTOs;
using OrderingServiceWeb.Helpers.Abstractions;

namespace OrderingServiceWeb.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ICustomerManager _customerManager;
        private readonly ISecurityHelper _securityHelper;

        public TokenController(ICustomerManager customerManager, ISecurityHelper securityHelper)
        {
            _customerManager = customerManager;
            _securityHelper = securityHelper;
        }

        [HttpGet("Token")]
        public ActionResult<string> Token(CustomerAuthenticationDto customerAuthenticationDto)
        {
            if (customerAuthenticationDto == null || string.IsNullOrWhiteSpace(customerAuthenticationDto.Email) || string.IsNullOrWhiteSpace(customerAuthenticationDto.Password))
            {
                return BadRequest("Invalid customer authentication data.");
            }

            if(_customerManager.ValidateCustomerCredentials(customerAuthenticationDto.Email, customerAuthenticationDto.Password))
            {
                CustomerModel temp = _customerManager.GetCustomer(customerAuthenticationDto.Email)!;
                string token = _securityHelper.GenerateToken(temp);

                return token;
            }

            return Unauthorized("Invalid email or password.");
        }
    }
}

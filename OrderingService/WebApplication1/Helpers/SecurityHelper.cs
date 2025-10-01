using Microsoft.IdentityModel.Tokens;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using OrderingServiceWeb.Helpers.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderingServiceWeb.Helpers
{
    public class SecurityHelper : ISecurityHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerManager _customerManager;
        private readonly IConfiguration _configuration;

        public SecurityHelper(IHttpContextAccessor httpContextAccessor, ICustomerManager customerManager, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _customerManager = customerManager;
            _configuration = configuration;
        }

        public long GetCustomerID()
        {
            return long.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public CustomerModel GetCustomer()
        {
            long customerID = long.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return _customerManager.GetCustomer(customerID)!;
        }

        public string GenerateToken(CustomerModel customer)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.ID.ToString()),
                new Claim(ClaimTypes.Name, customer.Email),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

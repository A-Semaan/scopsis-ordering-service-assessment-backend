using OrderingServiceEngine.Models;

namespace OrderingServiceWeb.Helpers.Abstractions
{
    public interface ISecurityHelper
    {
        public long GetCustomerID();

        public CustomerModel GetCustomer();

        public string GenerateToken(CustomerModel customer);
    }
}

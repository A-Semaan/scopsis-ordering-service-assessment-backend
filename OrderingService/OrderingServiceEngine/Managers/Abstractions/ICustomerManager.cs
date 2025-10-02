using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers.Abstractions
{
    public interface ICustomerManager
    {
        public long InsertCustomer(string name, string email, string password);
        
        public bool ValidateCustomerCredentials(string email, string password);
        public bool UpdateCustomer(long customerID, string? name, string? email, string? password);
        public List<CustomerModel> GetAllCustomers();
        public CustomerModel? GetCustomer(string email);
        public CustomerModel? GetCustomer(long customerID);
        public bool CustomerEmailExists(string email);
    }
}

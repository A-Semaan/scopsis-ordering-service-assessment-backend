using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess.Abstractions
{
    public interface ICustomerDataAccess
    {
        public long InsertCustomer(Customer customer);
        public bool UpdateCustomer(long customerID, string? name, string? email, string? passwordHash);
        public List<Customer> GetAllCustomers();
        public Customer? GetCustomer(string email);
        public Customer? GetCustomer(long customerID);
        public bool CustomerEmailExists(string email);
    }
}

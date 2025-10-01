using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess
{
    public class CustomerDataAccess : ICustomerDataAccess
    {
        private readonly OrderingServiceDbContext _dbContext;

        public CustomerDataAccess(OrderingServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CustomerEmailExists(string email)
        {
            Customer? customer = _dbContext.Customers.Where(c => c.Email == email).FirstOrDefault();

            return customer != null;
        }

        public Customer? GetCustomer(string email)
        {
            Customer? customer = _dbContext.Customers.Where(c => c.Email == email).FirstOrDefault();

            return customer;
        }

        public Customer? GetCustomer(long customerID)
        {
            Customer? customer = _dbContext.Customers.Find(customerID);

            return customer;
        }

        public long InsertCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);

            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to insert customer");
            }

            return customer.ID;
        }

        public bool UpdateCustomer(long customerID, string? name, string? email, string? passwordHash)
        {
            Customer? temp = _dbContext.Customers.Where(c => c.ID == customerID).FirstOrDefault();

            if (temp == null)
            {
                throw new Exception("Customer not found");
            }

            if (!string.IsNullOrWhiteSpace(name))
                temp.Name = name;

            if (!string.IsNullOrWhiteSpace(email))
                temp.Email = email;

            if (!string.IsNullOrWhiteSpace(passwordHash))
                temp.PasswordHash = passwordHash;

            return _dbContext.SaveChanges() > 0;
        }
    }
}

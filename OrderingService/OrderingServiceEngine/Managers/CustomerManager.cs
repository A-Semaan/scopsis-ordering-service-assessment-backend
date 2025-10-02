using AutoMapper;
using OrderingServiceData.DataAccess;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerDataAccess _customerDataAccess;

        private readonly IApplicationLogManager _applicationLogManager;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerDataAccess customerDataAccess, IApplicationLogManager applicationLogManager, IMapper mapper)
        {
            _customerDataAccess = customerDataAccess;
            _applicationLogManager = applicationLogManager;
            _mapper = mapper;
        }

        public long InsertCustomer(string name, string email, string password)
        {
            if (CustomerEmailExists(email))
            {
                throw new Exception("Email already in use");
            }
            ;

            Customer customer = new Customer
            {
                Name = name,
                Email = email,
                PasswordHash = Sha256Hash(password) // In a real application, ensure to hash the password properly
            };

            long customerID = _customerDataAccess.InsertCustomer(customer);
            if (customerID > 0)
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.CUSTOMER_CREATED, email, customerID);
                return customerID;
            }

            return -1;
        }

        public bool CustomerEmailExists(string email)
        {
            return _customerDataAccess.CustomerEmailExists(email);
        }

        public List<CustomerModel> GetAllCustomers()
        {
            return _mapper.Map<List<CustomerModel>>(_customerDataAccess.GetAllCustomers());
        }

        public CustomerModel? GetCustomer(string email)
        {
            return _mapper.Map<CustomerModel>(_customerDataAccess.GetCustomer(email));
        }

        public CustomerModel? GetCustomer(long customerID)
        {
            return _mapper.Map<CustomerModel>(_customerDataAccess.GetCustomer(customerID));
        }

        public bool UpdateCustomer(long customerID, string? name, string? email, string? password)
        {
            if (!string.IsNullOrWhiteSpace(email) && CustomerEmailExists(email))
            {
                throw new Exception("Email already in use");
            }

            string? hashedPassword = string.IsNullOrWhiteSpace(password) ? null : Sha256Hash(password);

            if (_customerDataAccess.UpdateCustomer(customerID, name, email, hashedPassword))
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.CUSTOMER_UPDATED, null, customerID);
                return true;
            }

            return false;
        }

        public bool ValidateCustomerCredentials(string email, string password)
        {
            string passwordHash = Sha256Hash(password);

            Customer? customer = _customerDataAccess.GetCustomer(email);

            if (customer == null)
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.LOGIN_FAILURE, email);
                return false;
            }
            if (customer.PasswordHash != passwordHash)
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.LOGIN_FAILURE, email, customer.ID);
                return false;
            }

            _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.LOGIN_SUCCESS, email, customer.ID);
            return true;
        }


        public static string Sha256Hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}

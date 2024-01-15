using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;

namespace Hotel.Domain.Managers
{
    public class CustomerManager
    {
        private ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                if (ValidateCustomerData(customer))
                {
                    _customerRepository.AddCustomer(customer);
                }
                else
                {
                    throw new CustomerManagerException("Invalid customer data.");
                }
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("Error adding customer", ex);
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            try
            {
                Customer customer = _customerRepository.GetCustomerById(customerId);

                if (customer != null)
                {
                    return customer;
                }
                else
                {
                    throw new CustomerManagerException($"Customer with ID {customerId} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("Error adding customer by ID", ex);
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                if (ValidateCustomerData(customer))
                {
                    _customerRepository.UpdateCustomer(customer);
                }
                else
                {
                    throw new CustomerManagerException("Invalid customer data.");
                }
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("Error updating customer", ex);
            }
        }

        public void DeleteCustomer(int customerId)
        {
            try
            {
                _customerRepository.DeleteCustomer(customerId);
            }
            catch (Exception ex)
            {
                throw new CustomerManagerException("Error deleting customer", ex);
            }
        }

        public bool ValidateCustomerData(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name) || customer.Name.Length > 500)
                return false;

            if (
                string.IsNullOrWhiteSpace(customer.Contact?.Email)
                || !customer.Contact.Email.Contains("@")
            )
                return false;

            if (string.IsNullOrWhiteSpace(customer.Contact?.Phone))
                return false;

            return true;
        }
    }
}

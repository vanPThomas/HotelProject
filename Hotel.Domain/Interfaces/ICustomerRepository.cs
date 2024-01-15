using Hotel.Domain.Model;

namespace Hotel.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        void DeleteCustomer(int customerId);
        void UpdateCustomer(Customer customer);
        Customer GetCustomerById(int customerId);
    }
}

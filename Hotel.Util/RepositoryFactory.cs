using Hotel.Domain.Interfaces;
using Hotel.Persistence.Repositories;
using System.Configuration;

namespace Hotel.Util
{
    public static class RepositoryFactory
    {
        public static ICustomerRepository CustomerRepository
        {
            get
            {
                return new CustomerRepository(
                    "Data Source = Himeko\\SQLEXPRESS; Initial Catalog = HotelDonderdag; Integrated Security = True"
                );
            }
        }
        public static IOrganisationRepository OrganisationRepository
        {
            get
            {
                return new OrganisationRepository(
                    "Data Source = Himeko\\SQLEXPRESS; Initial Catalog = HotelDonderdag; Integrated Security = True"
                );
            }
        }

        public static IActivityRepository ActivityRepository
        {
            get
            {
                return new ActivityRepository(
                    "Data Source = Himeko\\SQLEXPRESS; Initial Catalog = HotelDonderdag; Integrated Security = True"
                );
            }
        }
    }
}

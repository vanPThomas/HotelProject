using Hotel.Domain.Model;
using Hotel.Persistence.Repositories;

namespace ConsoleAppDL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = "Data Source=NB21-6CDPYD3\\SQLEXPRESS;Initial Catalog=HotelDonderdag;Integrated Security=True";
            CustomerRepository repo = new CustomerRepository(conn);
            //var x = repo.GetCustomers("ge");
            Customer c=new Customer("piet",new ContactInfo("piet@yahoo","013456",new Address("Gent","Kerkstraat","9000","185")));
            c.AddMember(new Member("paul", new DateOnly(2000, 5, 8)));
            c.AddMember(new Member("rudy",new DateOnly(1987,1,1)));
            repo.AddCustomer(c);

        }
    }
}
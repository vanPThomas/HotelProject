using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Model
{
    public class ContactInfo
    {
        public int Id { get; set; }
        private string _email;
        private string _phone;
        private Address _address;

        // constructors
        public ContactInfo() { }

        public ContactInfo(string email, string phone, Address address)
        {
            Email = email;
            Phone = phone;
            Address = address;
        }

        public ContactInfo(int id, string email, string phone, Address address)
        {
            Id = id;
            Email = email;
            Phone = phone;
            Address = address;
        }

        // getters and setters
        public string Email
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("Email cannot be empty or null.");
                _email = value;
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("Phone cannot be empty or null.");
                _phone = value;
            }
        }

        public Address Address
        {
            get { return _address; }
            set
            {
                if (value == null)
                    throw new CustomerException("Address cannot be null.");
                _address = value;
            }
        }
    }
}

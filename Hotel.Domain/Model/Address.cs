using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Model
{
    public class Address
    {
        public int Id { get; set; }
        private const char splitChar = '|';
        private string _city;
        private string _postalCode;
        private string _houseNumber;
        private string _street;

        // constructors
        public Address(string city, string street, string postalCode, string houseNumber)
        {
            City = city;
            Street = street;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
        }

        public Address(int id, string city, string street, string postalCode, string houseNumber)
        {
            Id = id;
            City = city;
            Street = street;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
        }

        public Address(string addressLine)
        {
            string[] parts = addressLine.Split(splitChar);
            City = parts[0];
            Street = parts[2];
            PostalCode = parts[1];
            HouseNumber = parts[3];
        }

        public Address(int id, string addressLine)
        {
            Id = id;
            string[] parts = addressLine.Split(splitChar);
            City = parts[0];
            Street = parts[2];
            PostalCode = parts[1];
            HouseNumber = parts[3];
        }

        public Address() { }

        // getters and setters
        public string City
        {
            get { return _city; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("Mun is empty");
                _city = value;
            }
        }
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("Zip is empty");
                _postalCode = value;
            }
        }
        public string HouseNumber
        {
            get { return _houseNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("HN is empty");
                _houseNumber = value;
            }
        }
        public string Street
        {
            get { return _street; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("Street is empty");
                _street = value;
            }
        }

        // rest
        public override string ToString()
        {
            return $"{City} [{PostalCode}] - {Street} - {HouseNumber}";
        }

        public string ToAddressLine()
        {
            return $"{City}{splitChar}{PostalCode}{splitChar}{Street}{splitChar}{HouseNumber}";
        }
    }
}

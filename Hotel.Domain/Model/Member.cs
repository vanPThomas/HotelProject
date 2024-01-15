using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Model
{
    public class Member
    {
        public int Id { get; set; }
        private string _name;
        private DateOnly _birthday;

        // constructors
        public Member() { }

        public Member(int id, string name, DateOnly birthday)
        {
            Id = id;
            Name = name;
            Birthday = birthday;
        }

        public Member(string name, DateOnly birthday)
        {
            Name = name;
            Birthday = birthday;
        }

        // getters and setters
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomerException("member");
                _name = value;
            }
        }

        public DateOnly Birthday
        {
            get { return _birthday; }
            set
            {
                if (DateOnly.FromDateTime(DateTime.Now) <= value)
                    throw new CustomerException("member");
                _birthday = value;
            }
        }

        // rest
        public override bool Equals(object? obj)
        {
            return obj is Member member
                && _name == member._name
                && _birthday.Equals(member._birthday);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, _birthday);
        }
    }
}

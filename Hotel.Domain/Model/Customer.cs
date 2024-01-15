using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContactInfo Contact { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();

        // constructors
        public Customer(int id, string name, ContactInfo contact)
        {
            Id = id;
            Name = name;
            Contact = contact;
        }

        public Customer(string name, ContactInfo contact)
        {
            Name = name;
            Contact = contact;
        }

        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Customer() { }

        //getters and setters
        public IReadOnlyList<Member> GetMembers()
        {
            return Members.AsReadOnly();
        }

        // change members
        public void AddMember(Member member)
        {
            if (!Members.Contains(member))
                Members.Add(member);
            else
                throw new CustomerException("AddMember");
        }

        public void RemoveMember(Member member)
        {
            if (Members.Contains(member))
                Members.Remove(member);
            else
                throw new CustomerException("RemoveMember");
        }
    }
}

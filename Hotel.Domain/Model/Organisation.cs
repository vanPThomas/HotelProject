namespace Hotel.Domain.Model
{
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContactInfo ContactInfo { get; set; }
        private List<Activity> _activities = new List<Activity>();

        public Organisation() { }

        public Organisation(string name, ContactInfo contactInfo)
        {
            Name = name;
            ContactInfo = contactInfo;
        }

        public Organisation(int id, string name, ContactInfo contactInfo)
        {
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
        }
    }
}

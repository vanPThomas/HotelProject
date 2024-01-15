namespace Hotel.Domain.Model
{
    public class Description
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public string TotalDescription { get; set; }

        public Description() { }

        public Description(string name, string location, int duration, string description)
        {
            Name = name;
            Location = location;
            Duration = duration;
            TotalDescription = description;
        }

        public Description(int id, string name, string location, int duration, string description)
        {
            Id = id;
            Name = name;
            Location = location;
            Duration = duration;
            TotalDescription = description;
        }
    }
}

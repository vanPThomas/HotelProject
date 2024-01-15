namespace Hotel.Domain.Model
{
    public class Registration
    {
        public int Id { get; set; }
        public List<Member> Members { get; set; } = new List<Member>();

        public Registration() { }

        public Registration(int id)
        {
            Id = id;
        }
    }
}

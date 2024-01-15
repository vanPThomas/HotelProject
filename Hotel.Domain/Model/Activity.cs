namespace Hotel.Domain.Model
{
    public class Activity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfPlaces { get; set; }
        private List<Registration> _Registrations = new List<Registration>();
        public Tarif Tarif { get; set; }
        public Description Description { get; set; }

        //constructors

        public Activity() { }

        public Activity(DateTime date, int numberOfPlaces, Tarif tarif, Description description)
        {
            Date = date;
            NumberOfPlaces = numberOfPlaces;
            Tarif = tarif;
            Description = description;
        }

        public Activity(
            int id,
            DateTime date,
            int numberOfPlaces,
            Tarif tarif,
            Description description
        )
        {
            Id = id;
            Date = date;
            NumberOfPlaces = numberOfPlaces;
            Tarif = tarif;
            Description = description;
        }
    }
}

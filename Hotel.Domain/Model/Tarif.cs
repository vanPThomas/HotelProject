namespace Hotel.Domain.Model
{
    public class Tarif
    {
        public int Id { get; set; }
        public double AdultTarif { get; set; }
        public double ChildTarif { get; set; }
        public double Discount { get; set; }
        public int AdultAge { get; set; }

        public Tarif() { }

        public Tarif(double adultTarif, double childTarif, double discount, int adultAge)
        {
            AdultTarif = adultTarif;
            ChildTarif = childTarif;
            Discount = discount;
            AdultAge = adultAge;
        }

        public Tarif(int id, double adultTarif, double childTarif, double discount, int adultAge)
        {
            Id = id;
            AdultTarif = adultTarif;
            ChildTarif = childTarif;
            Discount = discount;
            AdultAge = adultAge;
        }
    }
}

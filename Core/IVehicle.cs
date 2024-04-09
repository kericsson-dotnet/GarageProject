namespace Core
{
    public enum Fuels
    {
        Petrol,
        Diesel,
        Electricity,
        Jetfuel,
    }

    public interface IVehicle
    {
        public string RegNr { get; set; }
        public string Color { get; set; }
        public Fuels FuelType { get; set; }
    }
}

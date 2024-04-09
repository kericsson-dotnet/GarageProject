namespace Core.Vehicles;
public abstract class LandVehicle(string regNr, string color, Fuels fuelType) : IVehicle
{
    public string RegNr { get; set; } = regNr;
    public string Color { get; set; } = color;
    public Fuels FuelType { get; set; } = fuelType;
    public abstract int WheelCount { get; }
}

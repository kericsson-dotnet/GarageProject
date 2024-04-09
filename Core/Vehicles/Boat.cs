namespace Core.Vehicles;

public class Boat(string regNr, string color, Fuels fuelType, int length) : IVehicle
{
    public string RegNr { get; set; } = regNr;
    public string Color { get; set; } = color;
    public Fuels FuelType { get; set; } = fuelType;
    public int Length { get; set; } = length;
}
namespace Core.Vehicles;

public class Airplane(string regNr, string color, Fuels fuelType, int numberOfEngines) : IVehicle
{
    public string RegNr { get; set; } = regNr;
    public string Color { get; set; } = color;
    public Fuels FuelType { get; set; } = fuelType;
    public int NumberOfEngines { get; set; } = numberOfEngines;
}

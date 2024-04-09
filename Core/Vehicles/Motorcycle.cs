namespace Core.Vehicles;

public class Motorcycle(string regNr, string color, Fuels fuelType, int cylinderVolume) : LandVehicle(regNr, color, fuelType)
{
    public int CylinderVolume { get; set; } = cylinderVolume;
    public override int WheelCount => 2;
}

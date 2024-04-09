namespace Core.Vehicles;

public class Car(string regNr, string color, Fuels fuelType, int numberOfDoors) : LandVehicle(regNr, color, fuelType)
{
    public int NumberOfDoors { get; set; } = numberOfDoors;
    public override int WheelCount => 4;

}

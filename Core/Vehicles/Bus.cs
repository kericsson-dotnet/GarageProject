namespace Core.Vehicles;

public class Bus(string regNr, string color, Fuels fuelType, int numberOfSeats) : LandVehicle(regNr, color, fuelType)
{
    public int NumberOfSeats { get; set; } = numberOfSeats;
    public override int WheelCount => 6;

}

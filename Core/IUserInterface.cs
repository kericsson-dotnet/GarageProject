using System.Reflection;

namespace Core;

public interface IUserInterface
{
    void MainMenu(GarageHandler handler);
    void PrintVehicleProperties(IVehicle vehicle);

    IVehicle CreateNewVehicle();

    Dictionary<string, object> AddPropertyFilter(List<PropertyInfo> uniqueProperties,
        Dictionary<string, object> searchProperties);

    void PopulateGarage(GarageHandler handler);
}
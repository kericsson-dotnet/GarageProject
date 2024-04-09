using System.Reflection;

namespace Core;

public interface IGarageHandler
{
    string ParkVehicle(IVehicle vehicle);
    string RemoveVehicle(string regNr);
    IVehicle GetVehicleByRegNr(string regNr);
    Dictionary<string, int> GetParkedTypeList();
    List<IVehicle> SearchVehicles(Dictionary<string, object> properties);

}
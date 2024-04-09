using System.Reflection;

namespace Core;

public class GarageHandler : IGarageHandler
{
    private Garage<IVehicle> _garage;

    public GarageHandler(Garage<IVehicle> garage)
    {
        _garage = garage;
    }

    public int GetTotalSlots()
    {
        return _garage.TotalSlots;
    }


    public string ParkVehicle(IVehicle vehicle)
    {
        return _garage.ParkVehicle(vehicle);
    }

    public string RemoveVehicle(string regNr)
    {
       return _garage.RemoveVehicle(regNr);
    }

    public IVehicle GetVehicleByRegNr(string regNr)
    {
        foreach (var vehicle in _garage.ParkedVehicles)
        {
            if (vehicle != null && vehicle.RegNr == regNr)
            {
                return vehicle;
            }
        }

        throw new KeyNotFoundException("Vehicle with that registration number not found");
    }

    public Dictionary<string, int> GetParkedTypeList()
    {
        var vehicleTypeList = new Dictionary<string, int>();
        foreach (var vehicle in _garage)
        {
            string vehicleType = vehicle.GetType().Name;
            if (vehicleTypeList.Keys.Contains(vehicleType))
            {
                vehicleTypeList[vehicleType]++;
            }
            else
            {
                vehicleTypeList.Add(vehicleType, 1);
            }
        }

        return vehicleTypeList;
    }

    public static List<PropertyInfo> GetUniqueVehicleProperties(List<Type> vehicleTypes)
    {
        var allProperties = new List<PropertyInfo>();
        foreach (var vehicleType in vehicleTypes)
        {
            allProperties.AddRange(vehicleType.GetProperties());
        }

        var uniquePropertyNames = allProperties.GroupBy(p => p.Name).Select(g => g.First()).ToList();
        return uniquePropertyNames;
    }

    public static List<Type> GetVehicleTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IVehicle).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .ToList();
    }

   public List<IVehicle> SearchVehicles(Dictionary<string, object> properties)
{
    List<IVehicle> matchingVehicles = new List<IVehicle>();

    foreach (var vehicle in _garage)
    {
        if (vehicle == null) continue;
        bool isMatch = true;
        foreach (var property in properties)
        {
            PropertyInfo vehicleProperty = vehicle.GetType().GetProperty(property.Key);
            var vehiclePropertyValue = vehicleProperty.GetValue(vehicle);
            if (vehiclePropertyValue is Enum && property.Value is Enum)
            {
                if (!vehiclePropertyValue.ToString().Equals(property.Value.ToString()))
                {
                    isMatch = false;
                    break;
                }
            }
            else
            {
                if (!vehiclePropertyValue.Equals(property.Value))
                {
                    isMatch = false;
                    break;
                }
            }
        }

        if (isMatch)
        {
            matchingVehicles.Add(vehicle);
        }
    }

    return matchingVehicles;
}
}
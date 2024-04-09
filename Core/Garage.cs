using System.Collections;
using System.Linq;

namespace Core;

public class Garage<T> : IEnumerable where T : IVehicle
{
    private readonly int _totalSlots;
    private IVehicle[] _parkedVehicles;

    public int TotalSlots => _totalSlots;
    public IVehicle[] ParkedVehicles => _parkedVehicles;

    public Garage(int totalSlots)
    {
        _totalSlots = totalSlots;
        _parkedVehicles = new IVehicle[totalSlots];
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T vehicle in _parkedVehicles)
        {
            if (vehicle != null)
            {
                yield return vehicle;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public string ParkVehicle(IVehicle vehicle)
    {
        if (_parkedVehicles.Count(nonNullSlots => nonNullSlots != null) >= _totalSlots)
        {
            throw new InvalidOperationException("Garage is full");
        }

        foreach (var parkedVehicle in _parkedVehicles)
        {
            if (parkedVehicle != null && parkedVehicle.RegNr == vehicle.RegNr)
            {
                throw new InvalidOperationException("Vehicle with this registration number is already parked");
            }
        }

        for (var i = 0; i < _totalSlots; i++)
        {
            if (_parkedVehicles[i] != null) continue;
            _parkedVehicles[i] = vehicle;
            return vehicle.RegNr;
        }
        throw new InvalidOperationException("Could not find a free slot in the garage");
    }

    public string RemoveVehicle(string regNr)
    {
        for (var i = 0; i < _totalSlots; i++)
        {
            if (_parkedVehicles[i] == null || _parkedVehicles[i].RegNr != regNr) continue;
            _parkedVehicles[i] = null;
            return regNr;
        }
        throw new InvalidOperationException("Registration number not found in garage");
    }

}
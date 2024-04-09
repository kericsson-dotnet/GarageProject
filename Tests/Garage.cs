using Core;
using Core.Vehicles;

namespace Tests;

public class Garage
{
    [Fact]
    public void ParkVehicle_AddVehicleToGarage_ShouldAdd()
    {
        // Arrange
        var garage = new Garage<IVehicle>(10);
        var car = new Car("ABC123", "Red", Fuels.Diesel, 4);

        // Act
        garage.ParkVehicle(car);
        var carInGarage = garage.ParkedVehicles.First();

        // Assert

        Assert.Equal(car, carInGarage);
    }

    [Fact]
    public void ParkVehicle_GarageIsFull_ShouldThrow()
    {
        // Arrange
        var garage = new Garage<IVehicle>(1);
        garage.ParkVehicle(new Car("ABC123", "Red", Fuels.Diesel, 4));

        // Act & Assert

        Assert.Throws<InvalidOperationException>(() => garage.ParkVehicle(new Car("ABC124", "Red", Fuels.Diesel, 4)));
    }

    [Fact]
    public void ParkVehicle_AddSameRegNr_ShouldThrow()
    {
        // Arrange
        var garage = new Garage<IVehicle>(1);
        garage.ParkVehicle(new Car("ABC123", "Red", Fuels.Diesel, 4));

        // Act & Assert

        Assert.Throws<InvalidOperationException>(() => garage.ParkVehicle(new Car("ABC123", "Red", Fuels.Diesel, 4)));
    }

    [Fact]
    public void RemoveVehicle_RemoveVehicleFromGarage_ShouldRemove()
    {
        // Arrange
        var garage = new Garage<IVehicle>(10);
        var car1 = new Car("ABC123", "Red", Fuels.Diesel, 4);
        var car2 = new Car("ABC124", "Red", Fuels.Diesel, 4);
        garage.ParkVehicle(car2);
        garage.ParkVehicle(car1);

        // Act
        garage.RemoveVehicle("ABC123");
        var carInGarage = garage.ParkedVehicles.Where(vehicle => vehicle != null).First();

        // Assert

        Assert.Equal(car2, carInGarage);
    }

    [Fact]
    public void RemoveVehicle_RemoveVehicleFromGarageWrongRegNr_ShouldThrow()
    {
        // Arrange
        var garage = new Garage<IVehicle>(10);
        var car1 = new Car("ABC123", "Red", Fuels.Diesel, 4);
        var car2 = new Car("ABC124", "Red", Fuels.Diesel, 4);
        garage.ParkVehicle(car1);
        garage.ParkVehicle(car2);

        // Act
        garage.RemoveVehicle("ABC123");

        // Assert

        Assert.Throws<InvalidOperationException>(() => garage.RemoveVehicle("ABC999"));
    }

    [Fact]
    public void Garage_TestEnumerator_ShouldEnumerate()
    {
        // Arrange
        var garage = new Garage<IVehicle>(10);
        garage.ParkVehicle(new Car("ABC123", "Red", Fuels.Diesel, 4));
        garage.ParkVehicle(new Car("ABC124", "Red", Fuels.Diesel, 4));
        garage.ParkVehicle(new Car("ABC125", "Red", Fuels.Diesel, 4));
        var ListOfCars = new List<IVehicle>();
        
        // Act
        foreach (var vehicle in garage)
        {
            ListOfCars.Add(vehicle);
        }
        // Assert

        Assert.Equal(3, ListOfCars.Count);
    }
}
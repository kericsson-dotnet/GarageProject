using System.Reflection;
using Core;
using Core.Vehicles;

namespace ConsoleUI;

public class UI : IUserInterface
{
    public Garage<IVehicle> CreateGarage()
    {
        var slotCount = GetNumber("Please enter the number of slots for the garage:");
        Console.WriteLine($"Garage created with {slotCount} slots.");

        return new Garage<IVehicle>(slotCount);
    }

    public void MainMenu(GarageHandler handler)
    {
        var menuItems = new List<string>
        {
            "Park a vehicle",
            "Remove a parked vehicle",
            "List number of vehicles in the garage by type",
            "Print properties of a vehicle in the garage",
            "Search for vehicles by property",
            "Quit application",
        };
        var menuChoice = ChooseFromList(menuItems);

        Console.WriteLine(menuChoice);
        switch (menuChoice)
        {
            case "Park a vehicle":
                try
                {
                    var addedRegNr = handler.ParkVehicle(CreateNewVehicle());
                    Console.WriteLine($"Vehicle with registration number {addedRegNr} added.");
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }

                break;
            case "Remove a parked vehicle":
                try
                {
                    var removedRegNr = handler.RemoveVehicle(GetRegNr());
                    {
                        Console.WriteLine($"Vehicle with registration number {removedRegNr} removed.");
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }

                break;
            case "List number of vehicles in the garage by type":
                Console.WriteLine("Vehicles in the garage by type:");
                var vehicleTypes = handler.GetParkedTypeList();
                foreach (var vehicleType in vehicleTypes)
                {
                    Console.WriteLine($"{vehicleType.Key}: {vehicleType.Value}");
                }

                break;
            case "Print properties of a vehicle in the garage":
                try
                {
                    var vehicle = handler.GetVehicleByRegNr(GetRegNr());
                    PrintVehicleProperties(vehicle);
                }
                catch (KeyNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }

                break;
            case "Search for vehicles by property":
                var searchProperties = new Dictionary<string, object>();
                var uniqueProperties = GarageHandler.GetUniqueVehicleProperties(GarageHandler.GetVehicleTypes());
                List<IVehicle> matchingVehicles;
                while (true)
                {
                    searchProperties = AddPropertyFilter(uniqueProperties, searchProperties);

                    matchingVehicles = handler.SearchVehicles(searchProperties);

                    if (!ChooseYesNo("Add another filter? y/n"))
                    {
                        break;
                    }
                }

                foreach (var vehicle in matchingVehicles)
                {
                    PrintVehicleProperties(vehicle);
                }

                break;
            case "Quit application":
                Console.WriteLine("Quitting...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    private bool ChooseYesNo(string prompt = "Choose (y)es or (n)o:")
    {
        Console.WriteLine(prompt);
        while (true)
        {
            var key = Console.ReadKey(intercept: true).KeyChar;
            switch (key)
            {
                case 'y':
                    return true;
                case 'n':
                    return false;
                default:
                    Console.WriteLine("Invalid choice. Please enter y or n.");
                    break;
            }
        }
    }

    private T ChooseFromList<T>(List<T> menuItems, string prompt = "Choose from the list:")
    {
        Console.WriteLine(prompt);
        while (true)
        {
            for (int i = 0; i < menuItems.Count(); i++)
            {
                if (menuItems[i] is Type type)
                {
                    Console.WriteLine($"{i + 1}. {type.Name}");
                }
                else if (menuItems[i] is PropertyInfo property)
                {
                    Console.WriteLine($"{i + 1}. {property.Name}");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {menuItems[i]}");
                }
            }

            if (int.TryParse(Console.ReadKey(intercept: true).KeyChar.ToString(), out var choice) && choice > 0 &&
                choice < menuItems.Count + 1)
            {
                return menuItems[choice - 1];
            }

            Console.WriteLine("Invalid choice. Please choose from the list.");
        }
    }

    private int GetNumber(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out var number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Invalid input. Please enter a valid positive number.");
        }
    }

    public void PrintVehicleProperties(IVehicle vehicle)
    {
        Type type = vehicle.GetType();
        PropertyInfo[] properties = type.GetProperties();

        Console.WriteLine($"Vehicle type: {type.Name}");
        foreach (PropertyInfo property in properties)
        {
            Console.WriteLine($"{property.Name}: {property.GetValue(vehicle)}");
        }
    }

    public static T GetEnumMember<T>(string prompt) where T : Enum
    {
        while (true)
        {
            Console.WriteLine(prompt);
            foreach (var member in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{(int)member + 1}. {member}");
            }

            if (uint.TryParse(Console.ReadKey().KeyChar.ToString(), out uint choice) &&
                choice <= Enum.GetValues(typeof(T)).Length)
            {
                return (T)Enum.GetValues(typeof(T)).GetValue((int)choice - 1);
            }

            Console.WriteLine("Invalid choice. Please choose from the list.");
        }
    }

    public IVehicle CreateNewVehicle()
    {
        Type vehicleType = ChooseFromList(GarageHandler.GetVehicleTypes(), "Select vehicle type:");

        string regNr = GetRegNr();

        Console.WriteLine("Enter color:");
        var color = Console.ReadLine();

        Fuels fuelType = GetEnumMember<Fuels>("Select fuel type:");

        var uniquePropertyName = vehicleType.GetProperties();
        int uniqueProperty = 0;

        // Get the unique property of the vehicle type
        foreach (var property in uniquePropertyName)
        {
            if (property.Name != "RegNr" && property.Name != "Color" && property.Name != "FuelType" &&
                property.Name != "WheelCount")

            {
                Console.WriteLine();
                uniqueProperty = GetNumber($"Enter value for {property.Name}:");
            }
        }

        return (IVehicle)Activator.CreateInstance(vehicleType, new object[]
        {
            regNr, color, fuelType, uniqueProperty
        });
    }

    private string GetRegNr()
    {
        string regNr;
        while (true)
        {
            Console.WriteLine("Enter registration number:");
            try
            {
                regNr = Helpers.ValidateRegNr(Console.ReadLine());
                break;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return regNr;
    }

    public Dictionary<string, object> AddPropertyFilter(List<PropertyInfo> uniqueProperties,
        Dictionary<string, object> searchProperties)
    {
        PropertyInfo searchProperty = ChooseFromList(uniqueProperties, "Select a property to filter by:");

        if (searchProperty.Name == "RegNr")
        {
            var regNr = GetRegNr();
            searchProperties.Add(searchProperty.Name, regNr);
        }

        else if (searchProperty.PropertyType == typeof(int))
        {
            // Console.WriteLine($"Enter a number for {searchProperty.Name}:");
            searchProperties.Add(searchProperty.Name, GetNumber($"Enter a number for {searchProperty.Name}:"));
        }
        else if (searchProperty.PropertyType == typeof(Fuels))
        {
            searchProperties.Add(searchProperty.Name, GetEnumMember<Fuels>("Select fuel type:"));
        }
        else
        {
            Console.WriteLine($"Enter the text value for {searchProperty.Name}:");
            searchProperties.Add(searchProperty.Name, Console.ReadLine());
        }

        return searchProperties;
    }

    public void PopulateGarage(GarageHandler handler)
    {
        if (ChooseYesNo("Would you like to pre-populate the garage with some vehicles? y/n"))
        {
            var vehicleList = new List<IVehicle>
            {
                new Car("AEG153", "Red", Fuels.Electricity, 5),
                new Car("BDG193", "Blue", Fuels.Petrol, 3),
                new Car("MIS517", "Yellow", Fuels.Petrol, 3),
                new Motorcycle("GXQ112", "Black", Fuels.Petrol, 250),
                new Airplane("JQW123", "White", Fuels.Jetfuel, 4),
                new Bus("YIQ582", "Yellow", Fuels.Diesel, 52),
                new Boat("KQW123", "White", Fuels.Diesel, 25),
            };
            foreach (var vehicle in vehicleList)
            {
                handler.ParkVehicle(vehicle);
            }

            Console.WriteLine($"Added {vehicleList.Count} to the garage:");
        }
    }
}
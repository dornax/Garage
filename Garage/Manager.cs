using Garage.Vehicles;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Xml.Serialization;

namespace Garage
{
    internal class Manager
    {
        IGarageHandler handler = default!;
        IFileIO fileIO = default!;
        private IUI ui;
        private bool exitMenu = false;
        private int index = 0;
        private string vehicle = "";
        private string registrationNumber = "";
        private string vehicleColor = "";
        private int numberOfWheels = 0;
        private string vehicleFuel = "";
        private int cylinderVolume = 0;
        private int numberOfSeats;
        private float vehicleLength;
        private int numberOfEngines;
        bool search;

        Dictionary<int, string> vehicles = new Dictionary<int, string>() {
            { 1, "Airplane" } , { 2, "Boat" }, { 3, "Bus" }, { 4, "Car" }, { 5, "Motorcycle" }
        };

        Dictionary<int, string> fuelDict = new Dictionary<int, string>() {
            { 1, "Diesel" } , { 2, "Gasoline" }, { 3, "Electric" }
        };
        public Manager(IUI ui)
        {
            this.ui = ui;
            handler = new GarageHandler(20);
           
            fileIO = new FileIO(handler, ui);
                    }
        public void Run()
        {
            
            Manage();
        }

        private void Manage()
        {
            exitMenu = false;
            ui.CursorVisible = false;

            MainMenu();
            do
            {
                MainMenuCommand();
            } while (!exitMenu);
        }
        private void MainMenu()
        {
            ui.Clear();
            ui.WriteLine(
                "Main Menu                    \n" +
                "1. Create garage             \n" +
                "2. Seed Garage               \n" +
                "3. Sum of vehicles by type   \n" +
                "4. Read from file            \n" +
                "5. Write to file             \n" +
                "A. Add vehicle               \n" +
                "R. Remove vehicle            \n" +
                "F. Find Vehicle              \n" +
                "S. Search Vehicles           \n" +
                "Q. Quit                        ");

        }
        private void MainMenuCommand()
        {
            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    CreateGarage();
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    ui.ClearCommand();
                    SeedGarage();
                    ui.WriteInfo("Populating the garage with a few vehicles");
                    break;
                case ConsoleKey.A:

                    search = false;
                    AddVehicle();
                    break;
                case ConsoleKey.F:
                    FindVehicle();
                    ui.ClearCommand();
                    break;
                case ConsoleKey.R:
                    RemoveVehicle();
                    break;
                case ConsoleKey.S:
                    exitMenu = true;
                    SearchMenu();
                    exitMenu = false;
                    do { SearchMenuCommand(); } while (!exitMenu);
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    SumOfVehiclesByType();
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    fileIO.ReadFromFile(handler, "Test.txt");
                    break;
                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    fileIO.WriteToFile(handler, "Test.txt");
                    break;
                case ConsoleKey.T:

                    break;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }
       
        private void SumOfVehiclesByType()
        {
            if (GarageIsEmpty()) { return; }
            ui.ClearInfo(1);
            ui.SetCursorPosition(0, ui.InfoRow);
            ui.WriteLine($"{handler.CountAllVehicles()}");
            ui.WriteLine("Press any key to clear");
            ui.GetKey();
            ui.ClearInfo(7);
        }
        private void SearchMenu()
        {
            ui.Clear();
            ui.WriteLine(
                "Search Menu                                     \n" +
                "1. Vehicle type by color and number of wheels   \n" +
                "2. Vehicle by color                             \n" +
                "3. Vehicle  by type                             \n" +
                "4. All vehicles by color                        \n" +
                "5. All vehicles                                 \n" +
                "Escape to return to main menu                    ");
        }
        private void SearchMenuCommand()
        {
            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {

                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    //Search Vehicle type by color and number of wheels
                    SearchVehicleByColorAndWheels();
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    //Search Vehicles by color 
                    SearchVehicleByColor();
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    //Search Vehicles by type
                    SearchVehicleByType();
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    //All vehicles by color
                    SearchAllVehiclesByColor();
                    break;
                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    SearchAll();
                    break;
                case ConsoleKey.D6 or ConsoleKey.NumPad6:

                    break;
                case ConsoleKey.Escape:
                    exitMenu = true;
                    Manage();
                    break;
            }
        }
        private void SearchAllVehiclesByColor()
        {
            if (GarageIsEmpty()) return;
            search = true;
            ui.Clear();
            GetColor();

            ui.Clear();
            string str = handler.GetByColor(vehicleColor);
            if (str == "") ui.WriteLine($"Did not find any {vehicleColor} vehicles\n");
            else ui.WriteLine(str);
            AnyKeyToSearchMenu();


        }
        private void SearchVehicleByType()
        {
            if (GarageIsEmpty()) return;
            search = true;
            ui.Clear();
            SelectVehicle();
            ui.Clear();
            string str = handler.GetAll(vehicle);
            if (str == "") ui.WriteLine($"Did not find any {vehicle}\n");
            else ui.WriteLine(str);
            AnyKeyToSearchMenu();
        }
        private void SearchVehicleByColor()
        {
            if (GarageIsEmpty()) return;
            search = true;
            ui.Clear();
            SelectVehicle();

            GetColor();
            ui.Clear();
            string str = handler.GetByColor(vehicle, vehicleColor);
            if (str == "") ui.WriteLine($"Did not find any {vehicleColor} {vehicle}\n");
            else ui.WriteLine(str);
            AnyKeyToSearchMenu();


        }
        private void SearchVehicleByColorAndWheels()
        {
            if (GarageIsEmpty()) return;
            search = true;
            ui.Clear();
            SelectVehicle();

            GetColor();
            GetWheels();
            ui.Clear();
            string str = handler.GetAll(vehicle, vehicleColor, numberOfWheels);
            if (str == "") ui.WriteLine($"Did not find any {vehicleColor} {vehicle} with {numberOfWheels} wheels\n");
            else ui.WriteLine(str);
            AnyKeyToSearchMenu();
        }
        private void SearchAll()
        {
            if (GarageIsEmpty()) return;
            ui.Clear();
            ui.SetCursorPosition(0, 0);
            ui.WriteLine(handler.GetAll());
            AnyKeyToSearchMenu();
        }
        private void AnyKeyToSearchMenu()
        {
            ui.WriteLine("Press any key to return to menu");
            ui.GetKey();
            SearchMenu();
        }
        private void CreateGarage()
        {
            ui.ClearInfo(2);
            ui.SetCursorPosition(0, ui.InfoRow);
            ui.Write($"Current garage have {handler.SizeOfGarage} spaces. Do you want\n" +
                "to erase current and create a new Y/n? ");

            string input = ui.GetInput(1);

            if (input == "Y")
            {
                ui.SetCursorPosition(0, ui.InfoRow + 3);
                string label = "Number of parking spaces: ";
                ui.Write(label);

                input = ui.GetDigit(5);

                if (int.TryParse(input, out int value))
                {
                    handler = new GarageHandler(value);
                    ui.ClearInfo(4);
                    ui.WriteInfo($"New garage created with {value} spaces");
                }
            }
            else ui.ClearInfo(2);
        }
        private void WriteEnterToConfirm() => ui.WriteInfo("Press Enter to confirm.");
        private void FindVehicle()
        {
            ui.ClearCommand();
            ui.ClearInfo(1);
            if (GarageIsEmpty()) { return; }
            ui.WriteInfo("Find Vehicle. Press Enter to confirm.");
            ui.WriteRegistrationLabel(0, ui.CommandRow);
            registrationNumber = ui.GetRegNum();

            bool success = false;
            while (!success)
            {
                bool validRegNum = ValidateRegNumLength(registrationNumber);
                if (validRegNum)
                {
                    registrationNumber = registrationNumber.ToUpper();
                    success = true;
                }
                else
                {
                    ui.SetCursorPosition(ui.LabelWidth + 1, ui.CommandRow);
                    registrationNumber = ui.GetRegNum();
                    success = false;
                }
            }
            string vehicle = handler.Find(registrationNumber);
            if (vehicle == "")
            {
                ui.ClearInfo(1);
                ui.WriteInfo($"{registrationNumber} not in the garage");
            }
            else
            {
                ui.ClearInfo(1);
                ui.ClearCommand();
                ui.WriteInfo(vehicle);
            }

        }
        private void AddVehicle()
        {
            if (IsGarageFull()) return;
            ui.Clear();
            ui.WriteEnterToConfirm();
            ui.WriteRegistrationLabel(0, 1);
            registrationNumber = ui.GetRegNum();

            bool success = false;
            do
            {
                if (ValidateRegNumLength(registrationNumber!))
                {
                    registrationNumber = registrationNumber!.ToUpper();
                    if (handler.Find(registrationNumber) != "")
                    {
                        ui.ClearInfo(1);
                        ui.WriteInfo($"{registrationNumber} is already in the garage");
                        ui.SetCursorPosition(ui.LabelWidth + 1, 1);
                        registrationNumber = ui.GetRegNum();
                        success = false;
                    }
                    else success = true;
                }
                else
                {
                    ui.SetCursorPosition(ui.LabelWidth + 1, 1);
                    registrationNumber = ui.GetRegNum();
                    success = false;
                }
            } while (!success);
            SelectVehicle();

        }
        private bool IsGarageFull()
        {
            if (handler.IsFull)
            {
                ui.ClearInfo(1);
                ui.WriteInfo("Garage is full");
                return true;
            }
            else return false;
        }
        private void SelectVehicle()
        {
            ui.ClearInfo(1);
            ui.CursorVisible = false;
            ui.SetCursorPosition(0, ui.InfoRow);
            ui.WriteColoredFirstLetter(DictionaryValueToList(vehicles));
            ui.WriteLine("Press first letter of vehicle to select type\n" +
                         "of vehicle, then press Enter to confirm.\n" +
                         "Press Escape to return to the menu.");
            ui.SetCursorPosition(0, 2);
            /*        123456789+123456789+123456789+12345679+                 */
            ui.Write(ui.PadLeft("Vehicle:", ui.LabelWidth));

            exitMenu = false;
            do { VehicleSelection(); } while (!exitMenu);
            exitMenu = false;
            if (!search) GetColor();
        }
        private void VehicleSelection()
        {
            List<string> vehicleList = new List<string>();
            vehicleList = DictionaryValueToList(vehicles);
            VehicleData vd = new();
            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {
                case ConsoleKey.A:
                    vehicle = "Airplane";
                    ui.SetCursorPosition(ui.LabelWidth + 1, 2);
                    ui.WriteLine(ui.PadRight(vehicle, 10));
                    break;
                case ConsoleKey.B:
                    IEnumerable<string> vehicles = vehicleList.Where(v => v.StartsWith("B"));
                    
                    int numberOfVehicles = vehicles.Count();
                    vehicle = vehicles.ElementAt(index);
                    ui.SetCursorPosition(ui.LabelWidth + 1, 2);
                    ui.WriteLine(ui.PadRight(vehicle, 10));
                    index++;
                    if (index == numberOfVehicles) index = 0;
                    break;
                case ConsoleKey.C:
                    ui.SetCursorPosition(ui.LabelWidth + 1, 2);
                    vehicle = "Car";
                    ui.Write(ui.PadRight(vehicle, 10));
                    break;
                case ConsoleKey.M:
                    vehicle = "Motorcycle";
                    ui.SetCursorPosition(ui.LabelWidth + 1, 2);
                    ui.Write($"{vehicle}");
                    break;
                case ConsoleKey.Enter:
                    exitMenu = true;
                    break;
                case ConsoleKey.Escape:
                    exitMenu = true;
                    if (search)
                    {
                        SearchMenu();
                        SearchMenuCommand();
                    }
                    else
                        Manage();
                    break;
            }
        }
        private void RemoveVehicle()
        {
            if (GarageIsEmpty()) { return; }
            ui.ClearInfo(1);
            ui.ClearCommand();
            ui.WriteInfo("Remove Vehicle. Press Enter to confirm, Escape to cancel.");
            ui.WriteRegistrationLabel(0, ui.CommandRow);

            registrationNumber = ui.GetRegNum();
            if (registrationNumber == null)
            {
                ui.ClearInfo(1);
                ui.ClearCommand();
                return;
            }
            bool success = false;
            while (!success)
            {
                bool validRegistrationNumber = ValidateRegNumLength(registrationNumber);
                if (validRegistrationNumber)
                {
                    registrationNumber = registrationNumber.ToUpper();
                    success = true;
                }
                else
                {
                    ui.ClearCommand();
                    ui.SetCursorPosition(ui.LabelWidth + 1, ui.CommandRow);
                    registrationNumber = ui.GetRegNum();
                    success = false;
                }
            }
            FindAndRemove();
        }
        private void FindAndRemove()
        {
            if (handler.Find(registrationNumber) == "")
            {
                ui.ClearInfo(1);
                ui.WriteInfo($"{registrationNumber} not in the garage");
            }
            else
            {
                handler.Remove(registrationNumber);
                ui.ClearInfo(1);
                ui.ClearCommand();
                ui.WriteInfo($"{registrationNumber} removed from the garage");
            }
        }
        private void GetColor()
        {
            int row = 3;
            ui.ClearInfo(4);
            ui.WriteEnterToConfirm();
            ui.SetCursorPosition(0, row);
            string label = ui.PadLeft("Color:", ui.LabelWidth) + " ";
            ui.Write(label);

            vehicleColor = ui.GetInput(20);

            bool success;
            do
            {
                string message = ValidateColor(vehicleColor!);
                if (message == "")
                {
                    success = true;
                }
                else
                {
                    ui.ClearInfo(1);
                    ui.WriteInfo(message);
                    ui.SetCursorPosition(ui.LabelWidth + 1, row);
                    ui.Write(ui.PadRight("", 20));
                    ui.SetCursorPosition(ui.LabelWidth + 1, row);
                    vehicleColor = ui.GetInput(20);
                    success = false;
                }
            } while (!success);
            if (!search)
            {
                if (vehicle == "Boat")
                {
                    numberOfWheels = 0;
                    GetLength();
                }
                else GetWheels();
            }
        }
        private void GetWheels()
        {
            ui.ClearInfo(4);
            ui.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 4);
            string label = ui.PadLeft("Wheels:", ui.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetDigit(2);
            int.TryParse(input, out numberOfWheels);
            if (!search)
            {
                if (vehicle == "Car") GetFuel();
                if (vehicle == "Bus") GetSeats();
                if (vehicle == "Motorcycle") GetCylinderVolume();
                if (vehicle == "Airplane") GetEngines();
            }
        }
        private void FuelSelection()
        {
            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {
                case ConsoleKey.D:
                    vehicleFuel = "Diesel";
                    WriteFuel(vehicleFuel, row: 5);
                    break;
                case ConsoleKey.E:
                    vehicleFuel = "Electric";
                    WriteFuel(vehicleFuel, row: 5);
                    break;
                case ConsoleKey.G:
                    vehicleFuel = "Gasoline";
                    WriteFuel(vehicleFuel, row: 5);
                    break;
                case ConsoleKey.Enter:
                    exitMenu = true;
                    break;
                case ConsoleKey.Escape:
                    exitMenu = true;
                    Manage();
                    break;
            }
        }
        private void WriteFuel(string fuel, int row)
        {
            ui.SetCursorPosition(ui.LabelWidth + 1, row);
            ui.WriteLine(ui.PadRight(fuel, 10));
        }
        private void GetFuel()
        {
            ui.SetCursorPosition(0, ui.InfoRow);
            ui.WriteColoredFirstLetter(DictionaryValueToList(fuelDict));
            ui.WriteLine("Press first letter of fuel to select type\n" +
                         "of fuel, then press Enter to confirm. Escape to cancel");

            ui.SetCursorPosition(0, 5);
            string label = ui.PadLeft("Fuel:", ui.LabelWidth) + " ";
            ui.Write(label);
            exitMenu = false;
            do { FuelSelection(); } while (!exitMenu);
            if (!search)
            {
                handler.Add(new Car(registrationNumber, vehicleColor, numberOfWheels, vehicleFuel));
                Manage();
            }
        }
        private void GetSeats()
        {
            ui.ClearInfo(4);
            ui.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            string label = ui.PadLeft("Seats:", ui.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetDigit(3);
            if (input == null)
            {
                Manage();
                return;
            }
            int.TryParse(input, out numberOfSeats);
            handler.Add(new Bus(registrationNumber, vehicleColor, numberOfWheels, numberOfSeats));
            Manage();
        }
        private void GetEngines()
        {
            ui.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            string label = ui.PadLeft("Engines:", ui.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetDigit(5);
            int.TryParse(input, out numberOfEngines);
            handler.Add(new Airplane(registrationNumber, vehicleColor, numberOfWheels, numberOfEngines));
            Manage();
        }
        private void GetLength()
        {
            ui.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 4);
            string label = ui.PadLeft("Length:", ui.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetValue(5);
            float.TryParse(input, out vehicleLength);
            handler.Add(new Boat(registrationNumber, vehicleColor, numberOfWheels, vehicleLength));
            Manage();
        }
        private void GetCylinderVolume()
        {
            ui.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            string label = ui.PadLeft("CylinderVolume:", ui.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetDigit(4);
            int.TryParse(input, out cylinderVolume);
            handler.Add(new Motorcycle(registrationNumber, vehicleColor, numberOfWheels, cylinderVolume));
            Manage();
        }
        private bool GarageIsEmpty()
        {
            if (handler.Count == 0)
            {
                ui.ClearInfo(1);
                ui.WriteInfo("Garage is empty");
                return true;
            }
            else
                return false;
        }
        private bool ValidateRegNumLength(string str)
        {
            if (str.Length == 6) return true;
            else
            {
                ui.ClearInfo(1);
                ui.WriteInfo("Registration Number should be 6 characters long.");
                return false;
            }
        }
        private string ValidateColor(string color)
        {
            string str = "";
            if (color != null)
            {
                if (color.Length > 2) str = "";
                else str = "The color should be at least 3 characters long.";
            }
            return str;
        }
        private void WriteInfo(string info)
        {
            ui.SetCursorPosition(0, ui.InfoRow);
            ui.WriteLine(info);
        }
        private List<string> DictionaryValueToList(Dictionary<int, string> dictionary)
        {
            List<string> list = new();
            for (int i = 1; i <= dictionary.Count; i++)
            {
                list.Add(dictionary.GetValueOrDefault(i)!);
            }
            return list;
        }
        private void SeedGarage()
        {
            handler.Add(new Airplane("FOO100", "Silver", 3, 2));
            handler.Add(new Airplane("FOO101", "Silver", 5, 4));
            handler.Add(new Car("AAA111", "Green", 4, "Gasoline"));
            handler.Add(new Car("ABB123", "Black", 4, "Diesel"));
            handler.Add(new Car("ABC222", "Silver Metallic", 4, "Electric"));
            handler.Add(new Car("BBC333", "Red", 4, "Gasoline"));
            handler.Add(new Car("CBA444", "White", 4, "Electric"));
            handler.Add(new Car("DEA555", "Red", 4, "Gasoline"));
            handler.Add(new Bus("ZOO110", "White", 6, 80));
            handler.Add(new Bus("ZOO111", "Red", 4, 30));
            handler.Add(new Boat("AND001", "White", 0, 10.5f));
            handler.Add(new Boat("AND011", "White", 0, 15));
            handler.Add(new Motorcycle("BAZ112", "Black", 2, 1200));
            handler.Add(new Motorcycle("BAZ111", "Red", 2, 1000));
        }
        private void Test()
        {
            SeedGarage();
            //string color = "Red";
            Func<IVehicle, bool> searchVehicleRed = v => v.GetType().Name == vehicle && v.Color == "Red";
            Func<IVehicle, bool> searchCar = v => v.GetType().Name == "Car";
            ui.WriteLine($"find ABC221 {handler.Find("ABC221")}");
            ui.WriteLine(handler.GetAll("Car"));
        }
        public string GetAll(Func<IVehicle, bool> search) => handler.GetAll(search);
    }

}

using Garage.Vehicles;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using System.Xml.Serialization;

namespace Garage
{
    internal class Manager
    {
        IGarageHandler handler = default!;
        IFileIO fileIO = default!;
        IHelperUI helperUI = default!;
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
            helperUI = new HelperUI(ui);
            fileIO = new FileIO(ui, helperUI, handler);
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
                    helperUI.ClearCommand();
                    SeedGarage();
                    helperUI.WriteInfo("Populating the garage with a few vehicles");
                    break;
                case ConsoleKey.A:

                    search = false;
                    AddVehicle();
                    break;
                case ConsoleKey.F:
                    FindVehicle();
                    helperUI.ClearCommand();
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
        private void ExecuteLine(string input)
        {
            string[] words = ConvertLine(input);
            string word = "", number = "";
            word = words[0];
            switch (word)
            {
                case "SizeOfGarage":
                    number = words[1];
                    int.TryParse(number, out int result);
                    if (result > handler.SizeOfGarage)
                        handler = new GarageHandler(result);
                    break;
                case "Airplane":
                    registrationNumber = words[1];
                    vehicleColor = words[2];
                    number = words[3];
                    int.TryParse(number, out numberOfWheels);
                    number = words[4];
                    int.TryParse(number, out numberOfEngines);
                    handler.Add(new Airplane(registrationNumber, vehicleColor, numberOfWheels, numberOfEngines));
                    break;
                case "Boat":
                    registrationNumber = words[1];
                    vehicleColor = words[2];
                    number = words[3];
                    int.TryParse(number, out numberOfWheels);
                    number = words[4];
                    float.TryParse(number, out vehicleLength);
                    handler.Add(new Boat(registrationNumber, vehicleColor, numberOfWheels, vehicleLength));
                    break;
                case "Bus":
                    registrationNumber = words[1];
                    vehicleColor = words[2];
                    number = words[3];
                    int.TryParse(number, out numberOfWheels);
                    number = words[4];
                    int.TryParse(number, out numberOfSeats);
                    handler.Add(new Bus(registrationNumber, vehicleColor, numberOfWheels, numberOfSeats));
                    break;
                case "Car":
                    registrationNumber = words[1];
                    vehicleColor = words[2];
                    number = words[3];
                    int.TryParse(number, out numberOfWheels);
                    vehicleFuel = words[4];
                    handler.Add(new Car(registrationNumber, vehicleColor, numberOfWheels, vehicleFuel));
                    break;
                case "Motorcycle":
                    registrationNumber = words[1];
                    vehicleColor = words[2];
                    number = words[3];
                    int.TryParse(number, out numberOfWheels);
                    number = words[4];
                    int.TryParse(number, out cylinderVolume);
                    handler.Add(new Motorcycle(registrationNumber, vehicleColor, numberOfWheels, cylinderVolume));
                    break;
            }
        }
        private string[] ConvertLine(string input)
        {
            string[] words = input!.Split('|');
            string[] modWords = new string[5];
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Contains('"')) modWords[i] = words[i].Replace('"', ' ').Trim();
                else modWords[i] = words[i].Trim();
            }
            return modWords;
        }
        private void SumOfVehiclesByType()
        {
            if (GarageIsEmpty()) { return; }
            helperUI.ClearInfo(1);
            ui.SetCursorPosition(0, helperUI.InfoRow);
            ui.WriteLine($"{handler.CountAllVehicles()}");
            ui.WriteLine("Press any key to clear");
            ui.GetKey();
            helperUI.ClearInfo(7);
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
            helperUI.ClearInfo(2);
            ui.SetCursorPosition(0, helperUI.InfoRow);
            ui.Write($"Current garage have {handler.SizeOfGarage} spaces. Do you want\n" +
                "to erase current and create a new Y/n? ");

            string input = ui.ReadLine();

            if (input == "Y")
            {
                ui.SetCursorPosition(0, helperUI.InfoRow + 3);
                string label = "Number of parking spaces: ";
                ui.Write(label);

                input = ui.GetDigit(5);

                if (int.TryParse(input, out int value))
                {
                    handler = new GarageHandler(value);
                    helperUI.ClearInfo(4);
                    helperUI.WriteInfo($"New garage created with {value} spaces");
                }
            }
            else helperUI.ClearInfo(2);
        }
        private void WriteEnterToConfirm() => helperUI.WriteInfo("Press Enter to confirm.");
        private void FindVehicle()
        {
            helperUI.ClearCommand();
            helperUI.ClearInfo(1);
            if (GarageIsEmpty()) { return; }
            helperUI.WriteInfo("Find Vehicle. Press Enter to confirm.");
            helperUI.WriteRegistrationLabel(0, helperUI.CommandRow);
            registrationNumber = ui.GetRegistrationNumber();

            bool success = false;
            while (!success)
            {
                bool validRegistrationNumber = ValidateRegNumberLength(registrationNumber);
                if (validRegistrationNumber)
                {
                    registrationNumber = registrationNumber.ToUpper();
                    success = true;
                }
                else
                {
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, helperUI.CommandRow);
                    ui.Write("      ");
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, helperUI.CommandRow);
                    registrationNumber = ui.GetRegistrationNumber();
                    success = false;
                }
            }
            string vehicle = handler.Find(registrationNumber);
            if (vehicle == "")
            {
                helperUI.ClearInfo(1);
                helperUI.WriteInfo($"{registrationNumber} not in the garage");
            }
            else
            {
                helperUI.ClearInfo(1);
                helperUI.ClearCommand();
                helperUI.WriteInfo(vehicle);
            }

        }
        private void AddVehicle()
        {
            if (IsGarageFull()) return;
            ui.Clear();
            helperUI.WriteEnterToConfirm();
            helperUI.WriteRegistrationLabel(0, 1);
            registrationNumber = ui.GetRegistrationNumber();

            bool success = false;
            do
            {
                if (ValidateRegNumberLength(registrationNumber!))
                {
                    registrationNumber = registrationNumber!.ToUpper();
                    if (handler.Find(registrationNumber) != "")
                    {
                        helperUI.ClearInfo(1);
                        helperUI.WriteInfo($"{registrationNumber} is already in the garage");
                        ui.SetCursorPosition(helperUI.LabelWidth + 1, 1);
                        ui.Write("      ");
                        ui.SetCursorPosition(helperUI.LabelWidth + 1, 1);
                        registrationNumber = ui.GetRegistrationNumber();
                        success = false;
                    }
                    else success = true;
                }
                else
                {
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, 1);
                    ui.Write("      ");
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, 1);
                    registrationNumber = ui.GetRegistrationNumber();
                    success = false;
                }
            } while (!success);
            SelectVehicle();

        }
        private bool IsGarageFull()
        {
            if (handler.IsFull)
            {
                helperUI.ClearInfo(1);
                helperUI.WriteInfo("Garage is full");
                return true;
            }
            else return false;
        }
        private void SelectVehicle()
        {
            helperUI.ClearInfo(1);
            ui.CursorVisible = false;
            ui.SetCursorPosition(0, helperUI.InfoRow);
            ui.WriteColoredFirstLetter(DictionaryValueToList(vehicles));
            ui.WriteLine("Press first letter of vehicle to select type\n" +
                         "of vehicle, then press Enter to confirm.\n" +
                         "Press Escape to return to the menu.");
            ui.SetCursorPosition(0, 2);
            /*        123456789+123456789+123456789+12345679+                 */
            ui.Write(helperUI.PadLeft("Vehicle:", helperUI.LabelWidth));

            exitMenu = false;
            do { VehicleSelection(); } while (!exitMenu);
            exitMenu = false;
            if (!search) GetColor();
        }
        private void VehicleSelection()
        {
            List<string> vehicleList = new List<string>();
            vehicleList = DictionaryValueToList(vehicles);

            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {
                case ConsoleKey.A:
                    vehicle = "Airplane";
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, 2);
                    ui.WriteLine(helperUI.PadRight(vehicle, 10));
                    break;
                case ConsoleKey.B:
                    IEnumerable<string> vehicles = vehicleList.Where(v => v.StartsWith("B"));
                    int numberOfVehicles = vehicles.Count();
                    vehicle = vehicles.ElementAt(index);
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, 2);
                    ui.WriteLine(helperUI.PadRight(vehicle, 10));
                    index++;
                    if (index == numberOfVehicles) index = 0;
                    break;
                case ConsoleKey.C:
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, 2);
                    vehicle = "Car";
                    ui.Write(helperUI.PadRight(vehicle, 10));
                    break;
                case ConsoleKey.M:
                    vehicle = "Motorcycle";
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, 2);
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
            helperUI.ClearInfo(1);
            helperUI.ClearCommand();
            helperUI.WriteInfo("Remove Vehicle. Press Enter to confirm, Escape to cancel.");
            helperUI.WriteRegistrationLabel(0, helperUI.CommandRow);

            registrationNumber = ui.GetRegistrationNumber();
            if (registrationNumber == null)
            {
                helperUI.ClearInfo(1);
                helperUI.ClearCommand();
                return;
            }
            bool success = false;
            while (!success)
            {
                bool validRegistrationNumber = ValidateRegNumberLength(registrationNumber);
                if (validRegistrationNumber)
                {
                    registrationNumber = registrationNumber.ToUpper();
                    success = true;
                }
                else
                {
                    helperUI.ClearCommand();
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, helperUI.CommandRow);
                    registrationNumber = ui.GetRegistrationNumber();
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, helperUI.CommandRow);
                    ui.Write("      ");
                    success = false;
                }
            }
            FindAndRemove();
        }
        private void FindAndRemove()
        {
            if (handler.Find(registrationNumber) == "")
            {
                helperUI.ClearInfo(1);
                helperUI.WriteInfo($"{registrationNumber} not in the garage");
            }
            else
            {
                handler.Remove(registrationNumber);
                helperUI.ClearInfo(1);
                helperUI.ClearCommand();
                helperUI.WriteInfo($"{registrationNumber} removed from the garage");
            }
        }
        private void GetColor()
        {
            int row = 3;
            helperUI.ClearInfo(4);
            helperUI.WriteEnterToConfirm();
            ui.SetCursorPosition(0, row);
            string label = helperUI.PadLeft("Color:", helperUI.LabelWidth) + " ";
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
                    helperUI.ClearInfo(1);
                    helperUI.WriteInfo(message);
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, row);
                    ui.Write(helperUI.PadRight("", 20));
                    ui.SetCursorPosition(helperUI.LabelWidth + 1, row);
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
            helperUI.ClearInfo(4);
            helperUI.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 4);
            string label = helperUI.PadLeft("Wheels:", helperUI.LabelWidth) + " ";
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
            ui.SetCursorPosition(helperUI.LabelWidth + 1, row);
            ui.WriteLine(helperUI.PadRight(fuel, 10));
        }
        private void GetFuel()
        {
            ui.SetCursorPosition(0, helperUI.InfoRow);
            ui.WriteColoredFirstLetter(DictionaryValueToList(fuelDict));
            ui.WriteLine("Press first letter of fuel to select type\n" +
                         "of fuel, then press Enter to confirm.");

            ui.SetCursorPosition(0, 5);
            string label = helperUI.PadLeft("Fuel:", helperUI.LabelWidth) + " ";
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
            helperUI.ClearInfo(4);
            helperUI.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            string label = helperUI.PadLeft("Seats:", helperUI.LabelWidth) + " ";
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
            helperUI.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            string label = helperUI.PadLeft("Engines:", helperUI.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetDigit(5);
            int.TryParse(input, out numberOfEngines);
            handler.Add(new Airplane(registrationNumber, vehicleColor, numberOfWheels, numberOfEngines));
            Manage();
        }
        private void GetLength()
        {
            helperUI.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 4);
            string label = helperUI.PadLeft("Length:", helperUI.LabelWidth) + " ";
            ui.Write(label);
            string input = ui.GetValue(5);
            float.TryParse(input, out vehicleLength);
            handler.Add(new Boat(registrationNumber, vehicleColor, numberOfWheels, vehicleLength));
            Manage();
        }
        private void GetCylinderVolume()
        {
            helperUI.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            string label = helperUI.PadLeft("CylinderVolume:", helperUI.LabelWidth) + " ";
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
                helperUI.ClearInfo(1);
                helperUI.WriteInfo("Garage is empty");
                return true;
            }
            else
                return false;
        }
        private bool ValidateRegNumberLength(string str)
        {
            if (str.Length == 6) return true;
            else
            {
                helperUI.ClearInfo(1);
                helperUI.WriteInfo("Registration Number should be 6 characters long.");
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
            ui.SetCursorPosition(0, helperUI.InfoRow);
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

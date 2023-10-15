using Garage.Vehicles;

namespace Garage
{
    internal class Manager
    {
        private IGarageHandler handler = default!;
        private IFileIO fileIO = default!;
        private IUI ui;
        private IUtil iUtil;
        private bool exitMenu = false;
        private int index = 0;
        private bool search;
        private VehicleData vd = new();

        //Dictionary<int, string> vehicles = new Dictionary<int, string>() {
        //    { 1, "Airplane" } , { 2, "Boat" }, { 3, "Bus" }, { 4, "Car" }, { 5, "Motorcycle" }
        //};

        public Manager(IUI ui)
        {
            this.ui = ui;
            this.iUtil = new Util(ui);
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
                    iUtil.ClearCommand();
                    SeedGarage();
                    iUtil.WriteInfo("Populating the garage with a few vehicles");
                    break;
                case ConsoleKey.A:
                    search = false;
                    AddVehicle();
                    break;
                case ConsoleKey.F:
                    FindVehicle();
                    iUtil.ClearCommand();
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
                    handler = fileIO.ReadFromFile(handler, iUtil,"Test.txt");
                    break;
                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    fileIO.WriteToFile(handler, iUtil, "Test.txt");
                    break;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
        }
        private void SumOfVehiclesByType()
        {
            if (GarageIsEmpty()) { return; }
            iUtil.ClearInfo(1);
            ui.SetCursorPosition(0, iUtil.InfoRow);
            ui.WriteLine($"{handler.CountAllVehicles()}");
            ui.WriteLine("Press any key to clear");
            ui.GetKey();
            iUtil.ClearInfo(7);
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
            string str = handler.GetByColor(vd.Color);
            if (str == "") ui.WriteLine($"Did not find any {vd.Color} vehicles\n");
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
            string str = handler.GetAll(vd.Vehicle);
            if (str == "") ui.WriteLine($"Did not find any {vd.Vehicle}\n");
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
            string str = handler.GetByColor(vd.Vehicle, vd.Color);
            if (str == "") ui.WriteLine($"Did not find any {vd.Color} {vd.Vehicle}\n");
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
            string str = handler.GetAll(vd.Vehicle, vd.Color, vd.Wheels);
            if (str == "") ui.WriteLine($"Did not find any {vd.Color} {vd.Vehicle} with {vd.Wheels} wheels\n");
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
            iUtil.ClearInfo(2);
            ui.SetCursorPosition(0, iUtil.InfoRow);
            ui.Write($"Current garage have {handler.SizeOfGarage} spaces. Do you want\n" +
                "to erase current and create a new Y/n? ");

            string input = ui.GetInput(1);

            if (input == "Y")
            {
                ui.SetCursorPosition(0, iUtil.InfoRow + 3);
                string label = "Number of parking spaces: ";
                ui.Write(label);

                input = ui.GetDigit(5);

                if (int.TryParse(input, out int value))
                {
                    handler = new GarageHandler(value);
                    iUtil.ClearInfo(4);
                    iUtil.WriteInfo($"New garage created with {value} spaces");
                }
            }
            else iUtil.ClearInfo(2);
        }
        private void FindVehicle()
        {
            iUtil.ClearCommand();
            iUtil.ClearInfo(1);
            if (GarageIsEmpty()) { return; }
            iUtil.WriteInfo("Find Vehicle. Press Enter to confirm.");
            iUtil.WriteRegistrationLabel(0, iUtil.CommandRow);
            vd.RegNum = ui.GetRegNum();

            bool success = false;
            while (!success)
            {
                bool validRegNum = ValidateRegNumLength(vd.RegNum);
                if (validRegNum)
                {
                    vd.RegNum = vd.RegNum.ToUpper();
                    success = true;
                }
                else
                {
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, iUtil.CommandRow);
                    vd.RegNum = ui.GetRegNum();
                    success = false;
                }
            }
            vd.Vehicle = handler.Find(vd.RegNum);
            if (vd.Vehicle == "")
            {
                iUtil.ClearInfo(1);
                iUtil.WriteInfo($"{vd.RegNum} not in the garage");
            }
            else
            {
                iUtil.ClearInfo(1);
                iUtil.ClearCommand();
                iUtil.WriteInfo(vd.Vehicle);
            }

        }
        private void AddVehicle()
        {
            if (IsGarageFull()) return;
            ui.Clear();
            iUtil.WriteEnterToConfirm();
            iUtil.WriteRegistrationLabel(0, 1);
            vd.RegNum = ui.GetRegNum();

            bool success = false;
            do
            {
                if (ValidateRegNumLength(vd.RegNum!))
                {
                    vd.RegNum = vd.RegNum!.ToUpper();
                    if (handler.Find(vd.RegNum) != "")
                    {
                        iUtil.ClearInfo(1);
                        iUtil.WriteInfo($"{vd.RegNum} is already in the garage");
                        ui.SetCursorPosition(iUtil.LabelWidth + 1, 1);
                        vd.RegNum = ui.GetRegNum();
                        success = false;
                    }
                    else success = true;
                }
                else
                {
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, 1);
                    vd.RegNum = ui.GetRegNum();
                    success = false;
                }
            } while (!success);
            vd.RegNum = vd.RegNum;
            SelectVehicle();

        }
        private void SelectVehicle()
        {
            iUtil.ClearInfo(1);
            ui.CursorVisible = false;
            ui.SetCursorPosition(0, iUtil.InfoRow);
            iUtil.WriteColoredFirstLetter(vd.VList);
            ui.WriteLine("Press first letter of vehicle to select type\n" +
                         "of vehicle, then press Enter to confirm.\n" +
                         "Press Escape to return to the menu.");
            ui.SetCursorPosition(0, 2);
            /*        123456789+123456789+123456789+12345679+                 */
            ui.Write(ui.PadLeft("Vehicle:", iUtil.LabelWidth));

            exitMenu = false;
            do { VehicleSelection(); } while (!exitMenu);
            exitMenu = false;
            if (!search) GetColor();
        }
        private void VehicleSelection()
        {
            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {
                case ConsoleKey.A:
                    vd.Vehicle = "Airplane";
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, 2);
                    ui.WriteLine(ui.PadRight(vd.Vehicle, 10));
                    break;
                case ConsoleKey.B:
                    IEnumerable<string> vehicles = vd.VList.Where(v => v.StartsWith("B"));
                    int numberOfVehicles = vehicles.Count();
                    vd.Vehicle = vehicles.ElementAt(index);
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, 2);
                    ui.WriteLine(ui.PadRight(vd.Vehicle, 10));
                    index++;
                    if (index == numberOfVehicles) index = 0;
                    break;
                case ConsoleKey.C:
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, 2);
                    vd.Vehicle = "Car";
                    ui.Write(ui.PadRight(vd.Vehicle, 10));
                    break;
                case ConsoleKey.M:
                    vd.Vehicle = "Motorcycle";
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, 2);
                    ui.Write($"{vd.Vehicle}");
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
            iUtil.ClearInfo(1);
            iUtil.ClearCommand();
            iUtil.WriteInfo("Remove Vehicle. Press Enter to confirm, Escape to cancel.");
            iUtil.WriteRegistrationLabel(0, iUtil.CommandRow);

            vd.RegNum = ui.GetRegNum();
            if (vd.RegNum == null)
            {
                iUtil.ClearInfo(1);
                iUtil.ClearCommand();
                return;
            }
            bool success = false;
            while (!success)
            {
                if (ValidateRegNumLength(vd.RegNum))
                {
                    vd.RegNum = vd.RegNum.ToUpper();
                    success = true;
                }
                else
                {
                    iUtil.ClearCommand();
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, iUtil.CommandRow);
                    vd.RegNum = ui.GetRegNum();
                    success = false;
                }
            }
            FindAndRemove();
        }
        private void FindAndRemove()
        {
            if (handler.Find(vd.RegNum) == "")
            {
                iUtil.ClearInfo(1);
                iUtil.WriteInfo($"{vd.RegNum} not in the garage");
            }
            else
            {
                handler.Remove(vd.RegNum);
                iUtil.ClearInfo(1);
                iUtil.ClearCommand();
                iUtil.WriteInfo($"{vd.RegNum} removed from the garage");
            }
        }
        private void GetColor()
        {
            int row = 3;
            iUtil.ClearInfo(4);
            iUtil.WriteEnterToConfirm();
            ui.SetCursorPosition(0, row);
            ui.Write($"{ui.PadLeft("Color:", iUtil.LabelWidth)} ");

            vd.Color = ui.GetInput(20);

            bool success;
            do
            {
                string message = ValidateColor(vd.Color!);
                if (message == "")
                {
                    success = true;
                }
                else
                {
                    iUtil.ClearInfo(1);
                    iUtil.WriteInfo(message);
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, row);
                    ui.Write(ui.PadRight("", 20));
                    ui.SetCursorPosition(iUtil.LabelWidth + 1, row);
                    vd.Color = ui.GetInput(20);
                    success = false;
                }
            } while (!success);
            if (!search)
            {
                if (vd.Vehicle == "Boat")
                {
                    vd.Wheels = 0;
                    GetLength();
                }
                else GetWheels();
            }
        }
        private void GetWheels()
        {
            int numberOfWheels;
            iUtil.ClearInfo(4);
            iUtil.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 4);
            ui.Write($"{ui.PadLeft("Wheels:", iUtil.LabelWidth)} ");
            string input = ui.GetDigit(2);
            int.TryParse(input, out numberOfWheels);
            vd.Wheels = numberOfWheels;
            if (!search)
            {
                if (vd.Vehicle == "Car") GetFuel();
                if (vd.Vehicle == "Bus") GetSeats();
                if (vd.Vehicle == "Motorcycle") GetCylinderVolume();
                if (vd.Vehicle == "Airplane") GetEngines();
            }
        }
        private void FuelSelection()
        {
            ConsoleKey keyPressed = ui.GetKey();
            switch (keyPressed)
            {
                case ConsoleKey.D:
                    WriteFuel(vd.Fuel = "Diesel", row: 5);
                    break;
                case ConsoleKey.E:
                    WriteFuel(vd.Fuel = "Electric", row: 5);
                    break;
                case ConsoleKey.G:
                    WriteFuel(vd.Fuel = "Gasoline", row: 5);
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
        public void WriteEnterToConfirm() => iUtil.WriteInfo("Press Enter to confirm.");
        private void WriteFuel(string fuel, int row)
        {
            ui.SetCursorPosition(iUtil.LabelWidth + 1, row);
            ui.WriteLine(ui.PadRight(fuel, 10));
        }
        private void GetFuel()
        {
            ui.SetCursorPosition(0, iUtil.InfoRow);
            iUtil.WriteColoredFirstLetter(vd.FuelList);
            ui.WriteLine("Press first letter of fuel to select type\n" +
                         "of fuel, then press Enter to confirm. Escape to cancel");

            ui.SetCursorPosition(0, 5);
            ui.Write($"{ui.PadLeft("Fuel:", iUtil.LabelWidth)} ");

            exitMenu = false;
            do { FuelSelection(); } while (!exitMenu);
            if (!search)
            {
                handler.Add(new Car(vd.RegNum, vd.Color, vd.Wheels, vd.Fuel));
                Manage();
            }
        }
        private void GetSeats()
        {
            int numberOfSeats;
            iUtil.ClearInfo(4);
            iUtil.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            ui.Write($"{ui.PadLeft("Seats:", iUtil.LabelWidth)} ");

            string input = ui.GetDigit(3);
            if (input == null)
            {
                Manage();
                return;
            }
            int.TryParse(input, out numberOfSeats);
            handler.Add(new Bus(vd.RegNum, vd.Color, vd.Wheels, vd.Seats = numberOfSeats));
            Manage();
        }
        private void GetEngines()
        {
            int numberOfEngines;
            iUtil.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            ui.Write($"{ui.PadLeft("Engines:", iUtil.LabelWidth)} ");
            string input = ui.GetDigit(2);
            int.TryParse(input, out numberOfEngines);
            handler.Add(new Airplane(vd.RegNum, vd.Color, vd.Wheels, vd.Engines = numberOfEngines));
            Manage();
        }
        private void GetLength()
        {
            float vehicleLength;
            iUtil.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 4);
            ui.Write($"{ui.PadLeft("Length:", iUtil.LabelWidth)} ");
            string input = ui.GetValue(5);
            float.TryParse(input, out vehicleLength);
            handler.Add(new Boat(vd.RegNum, vd.Color, vd.Wheels, vd.Length = vehicleLength));
            Manage();
        }
        private void GetCylinderVolume()
        {
            int cylinderVolume;
            iUtil.WriteEnterToConfirm();
            ui.SetCursorPosition(0, 5);
            ui.Write($"{ui.PadLeft("CylinderVolume:", iUtil.LabelWidth)} ");
            string input = ui.GetDigit(4);
            int.TryParse(input, out cylinderVolume);
            handler.Add(new Motorcycle(vd.RegNum, vd.Color, vd.Wheels, vd.CylinderVol = cylinderVolume));
            Manage();
        }
        private bool IsGarageFull()
        {
            if (handler.IsFull)
            {
                iUtil.ClearInfo(1);
                iUtil.WriteInfo("Garage is full");
                return true;
            }
            else return false;
        }
        private bool GarageIsEmpty()
        {
            if (handler.Count == 0)
            {
                iUtil.ClearInfo(1);
                iUtil.WriteInfo("Garage is empty");
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
                iUtil.ClearInfo(1);
                iUtil.WriteInfo("Registration Number should be 6 characters long.");
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
            Func<IVehicle, bool> searchVehicleRed = v => v.GetType().Name == "Car" && v.Color == "Red";
            Func<IVehicle, bool> searchCar = v => v.GetType().Name == "Car";
            ui.WriteLine($"find ABC221 {handler.Find("ABC221")}");
            ui.WriteLine(handler.GetAll("Car"));
        }
        public string GetAll(Func<IVehicle, bool> search) => handler.GetAll(search);
    }
}

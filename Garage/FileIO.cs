using Garage.Vehicles;

namespace Garage
{
    internal class FileIO : IFileIO
    {
       
        private IUI ui;
        private IGarageHandler _handler;
        public FileIO(IGarageHandler handler, IUI ui)
        {
            this.ui = ui;
            _handler = handler;
        }

        public IGarageHandler ReadFromFile(IGarageHandler handler, IUtil iUtil, string fileName)
        {
            _handler = handler;
            String line;
            StreamReader sr = default!;
            try
            {
                sr = new StreamReader(fileName);
                //Read the first line of text
                line = sr.ReadLine()!;
                //Continue to read until you reach end of file
                while (line != null)
                {
                    _handler = ExecuteLine(line);
                    line = sr.ReadLine()!;
                }
                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
            finally
            {
                sr.Close();
            }
            iUtil.ClearInfo(1);
            iUtil.WriteInfo($"{fileName} was read from disk. Press any key");
            ui.GetKey();
            iUtil.ClearInfo(1);
            return _handler;
        }
        public void WriteToFile(IGarageHandler handler, IUtil iUtil, string fileName)
        {
            StreamWriter sw = default!;
            try
            {
                sw = new StreamWriter(fileName);
                string fileToSave = handler.FileToSave();

                sw.Write(handler.FileToSave(), false);
                sw.Close();

            }
            catch (Exception e)
            {
                ui.WriteLine($"Exception: {e.Message}");
            }
            finally
            {
                sw.Close();
            }
            iUtil.ClearInfo(1);
            iUtil.WriteInfo($"{fileName} written to disk. Press any key.");
            ui.GetKey();
            iUtil.ClearInfo(1);
        }
        private IGarageHandler ExecuteLine(string input)
        {
            
            VehicleData vd = new();
            string[] words = ConvertLine(input);
            string word = "", number = "";
            word = words[0];

            if (word == "SizeOfGarage")
            {
                number = words[1];
                int.TryParse(number, out int result);
                if (result > _handler.SizeOfGarage)
                    _handler = new GarageHandler(result);
                
            }
            if (vd.VList.Contains(word))
            {
                vd.RegNum = words[1];
                vd.Color = words[2];
                number = words[3];
                int.TryParse(number, out int result);
                vd.Wheels = result;
                number = words[4];
            }
            switch (word)
            {
                case "Airplane":
                    int.TryParse(number, out int result);
                    _handler.Add(new Airplane(vd.RegNum, vd.Color, vd.Wheels, vd.Engines = result));
                    break;
                case "Boat":
                    float.TryParse(number, out float value);
                    _handler.Add(new Boat(vd.RegNum, vd.Color, vd.Wheels, vd.Length = value));
                    break;
                case "Bus":
                    int.TryParse(number, out result);
                    _handler.Add(new Bus(vd.RegNum, vd.Color, vd.Wheels, vd.Seats = result));
                    break;
                case "Car":
                    _handler.Add(new Car(vd.RegNum, vd.Color, vd.Wheels, vd.Fuel = words[4]));
                    break;
                case "Motorcycle":
                    number = words[4];
                    int.TryParse(number, out result);
                    _handler.Add(new Motorcycle(vd.RegNum, vd.Color, vd.Wheels, vd.CylinderVol = result));
                    break;
            }
            return _handler;
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
    }
}

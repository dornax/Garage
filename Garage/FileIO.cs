using Garage.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal class FileIO : IFileIO
    {
       
        private IUI ui;
        private IGarageHandler handler;

        VehicleData vd = new();
        
        public FileIO(IGarageHandler handler, IUI ui)
        {
            this.ui = ui;
            this.handler = handler;
        }

        public void ReadFromFile(IGarageHandler handler, string fileName)
        {
            
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
                    ExecuteLine(handler, line);
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
            ui.ClearInfo(1);
            ui.WriteInfo($"{fileName} was read from disk. Press any key");
            ui.GetKey();
            ui.ClearInfo(1);
        }
        public void WriteToFile(IGarageHandler handler, string fileName)
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
            ui.ClearInfo(1);
            ui.WriteInfo($"{fileName} written to disk. Press any key.");
            ui.GetKey();
            ui.ClearInfo(1);
        }
        private void ExecuteLine(IGarageHandler handler, string input)
        {
            string registrationNumber = "";
            string vehicleColor = "";
            int numberOfWheels = 0;
            string vehicleFuel = "";
            int cylinderVolume = 0;
            int numberOfSeats;
            float vehicleLength;
            int numberOfEngines;
            string[] words = ConvertLine(input);
            string word = "", number = "";
            word = words[0];

            if (word == "SizeOfGarage")
            {
                number = words[1];
                int.TryParse(number, out int result);
                if (result > handler.SizeOfGarage)
                    handler = new GarageHandler(result);
                return;
            }
            if (vd.VList.Contains(word))
            {
                registrationNumber = words[1];
                vehicleColor = words[2];
                number = words[3];
                int.TryParse(number, out numberOfWheels);
                number = words[4];
            }
            switch (word)
            {
                case "Airplane":
                    int.TryParse(number, out numberOfEngines);
                    handler.Add(new Airplane(registrationNumber, vehicleColor, numberOfWheels, numberOfEngines));
                    break;
                case "Boat":
                    int.TryParse(number, out numberOfWheels);
                    number = words[4];
                    float.TryParse(number, out vehicleLength);
                    handler.Add(new Boat(registrationNumber, vehicleColor, numberOfWheels, vehicleLength));
                    break;
                case "Bus":
                    int.TryParse(number, out numberOfSeats);
                    handler.Add(new Bus(registrationNumber, vehicleColor, numberOfWheels, numberOfSeats));
                    break;
                case "Car":
                    vehicleFuel = words[4];
                    handler.Add(new Car(registrationNumber, vehicleColor, numberOfWheels, vehicleFuel));
                    break;
                case "Motorcycle":
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

    }
}

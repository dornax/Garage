using Garage.Vehicles;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;

namespace Garage
{
    internal class GarageHandler : IGarageHandler
    {
        IGarage<IVehicle> garage;
        public int Count => garage.Count;
        public bool IsFull => garage.IsFull;
        public int SizeOfGarage => garage.SizeOfGarage;
        public GarageHandler(int sizeOfGarage) => garage = new Garage<IVehicle>(sizeOfGarage);
        public string Find(string registrationNumber)
        {
            string result = "";
            IVehicle vehicle = garage.FirstOrDefault(v => v.RegistrationNumber == registrationNumber)!;

            if (vehicle is null) result = "";
            else result = WriteVehicle(vehicle);
            
            return result;
        }
        public bool Add(IVehicle vehicle)
        {
            IVehicle test = garage.FirstOrDefault(v => v.RegistrationNumber == vehicle.RegistrationNumber)!;
            if (test is null)
                return garage.Add(vehicle);
            else
                return false;
        }
        public bool Remove(string registrationNumber)
        {
            IVehicle vehicle = garage.FirstOrDefault(v => v.RegistrationNumber == registrationNumber)!;
            if (vehicle is null)
            {
                return false;
            }
            else
            {
                garage.Remove(vehicle);
                return true;
            }
        }
        public string FileToSave()
        {
            string output = $"\"SizeOfGarage\"|{SizeOfGarage}\n";
            var vehicles = garage.OrderBy(v => v.RegistrationNumber).ToList();
            foreach (var item in vehicles)
            {
                output += StringToSave(item);
            }
            return output;
        }
        public string GetAll()
        {
            var vehicles = garage.OrderBy(v => v.RegistrationNumber).ToList();
            return WriteVehicle(vehicles);
        }
        public string GetByColor(string color)
        {
            IEnumerable<IVehicle> vehicles = garage
                .Where(v => v.Color == color)
                .OrderBy(x => x.RegistrationNumber).ToList();
            return WriteVehicle(vehicles);
        }
        public string GetByColor(string color, int wheel)
        {
            IEnumerable<IVehicle> vehicles = garage
                .Where(v => v.Color == color && v.NumberOfWheels == wheel)
                .OrderBy(x => x.RegistrationNumber).ToList();
            return WriteVehicle(vehicles);
        }
        public string GetByColor(string vehicle, string color)
        {
            IEnumerable<IVehicle> vehicles = garage
                .Where(v => v.GetType().Name == vehicle && v.Color == color)
                .OrderBy(x => x.RegistrationNumber).ToList();
            return WriteVehicle(vehicles);
        }
        public string GetAll(string vehicle)
        {
            IEnumerable<IVehicle> vehicles = garage
                .Where(v => v.GetType().Name == vehicle)
                .OrderBy(x => x.RegistrationNumber).ToList();
            return WriteVehicle(vehicles);
        }
        public string GetAll(string vehicle, int numberOfWheels)
        {
            IEnumerable<IVehicle> vehicles = garage
                .Where(v => v.GetType().Name == vehicle && v.NumberOfWheels == numberOfWheels)
                .OrderBy(x => x.RegistrationNumber).ToList(); ;
            return WriteVehicle(vehicles);
        }
        public string GetAll(string vehicle, string color, int wheel)
        {
            IEnumerable<IVehicle> vehicles = garage
                .Where(v => v.GetType().Name == vehicle && v.Color == color && v.NumberOfWheels == wheel)
                .OrderBy(x => x.RegistrationNumber).ToList(); ;
            string result = WriteVehicle(vehicles);
            return result;
        }
        public string GetAll(Func<IVehicle, bool> search)
        {
            string result = "";
            
            IEnumerable<IVehicle> vehicles = garage.Where(search);
            result = WriteVehicle(vehicles);
            return result;
        }
        private int NumberOfVehicles(string vehicle)
        {
            var vehicles = garage.Where(v => v.GetType().Name == vehicle);
            int numberOfVehicles = vehicles.Count();
            return numberOfVehicles;
        }
        public string CountAllVehicles()
        {
            string output = "";
            string vehicle = "";
            var names = garage.GroupBy(v => v.GetType().Name)
                .Select(x => x.First()).Distinct()
                .OrderBy(y => y.GetType().Name);
            foreach (IVehicle item in names)
            {
                vehicle = item.GetType().Name;
                output += $"   {vehicle,-10} {NumberOfVehicles(vehicle)} \n";
            }
            return output;
        }
        private string WriteVehicle(IVehicle vehicle) => VehicleToString(vehicle);
        private string WriteVehicle(IEnumerable<IVehicle> vehicles)
        {
            string result = "";

            foreach (var vehicle in vehicles)
            {
                result += VehicleToString(vehicle);
            }
            return result;
        }

        private string StringToSave(IVehicle vehicle) 
        {
            string output = "";
            if (vehicle is Airplane toAirplane)
                output = $"\"{vehicle}\"|\"{vehicle.RegistrationNumber}\"|\"{vehicle.Color}\"|{vehicle.NumberOfWheels}|{toAirplane.NumberOfEngines}\r\n";
            if (vehicle is Boat toBoat)
                output = $"\"{vehicle}\"|\"{vehicle.RegistrationNumber}\"|\"{vehicle.Color}\"|{vehicle.NumberOfWheels}|{toBoat.Length}\r\n";
            if (vehicle is Car toCar)
                output = $"\"{vehicle}\"|\"{vehicle.RegistrationNumber}\"|\"{vehicle.Color}\"|{vehicle.NumberOfWheels}|{toCar.Fuel}\r\n";
            if (vehicle is Motorcycle toMotorcycle)
                output = $"\"{vehicle}\"|\"{vehicle.RegistrationNumber}\"|\"{vehicle.Color}\"|{vehicle.NumberOfWheels}|{toMotorcycle.CylinderVolume}\r\n";
            if (vehicle is Bus toBus)
                output = $"\"{vehicle}\"|\"{vehicle.RegistrationNumber}\"|\"{vehicle.Color}\"|{vehicle.NumberOfWheels}|{toBus.NumberOfSeats}\r\n";
            return output;
        }
        private string VehicleToString(IVehicle vehicle)
        {
            string output = "";
            if (vehicle is Airplane toAirplane)
                output = $"{vehicle.Specs()} wheels Engines:{toAirplane.NumberOfEngines}\n";
            if (vehicle is Boat toBoat)
                output = $"{vehicle.RegistrationNumber} {vehicle} Color:{vehicle.Color} Length:{toBoat.Length}m\n";
            if (vehicle is Car toCar)
                output = $"{vehicle.Specs()} wheels {toCar.Fuel}\n";
            if (vehicle is Motorcycle toMotorcycle)
                output = $"{vehicle.Specs()} wheels {toMotorcycle.CylinderVolume}cc\n";
            if (vehicle is Bus toBus)
                output = $"{vehicle.Specs()} wheels Seats:{toBus.NumberOfSeats}\n";
            return output;
        }

    }
}

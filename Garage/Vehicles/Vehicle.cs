using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Vehicles
{
    internal partial class Vehicle : IVehicle
    {

        private string _registrationNumber;
        private string _color = "";
        private int _numberOfWheels;

        public string RegistrationNumber { get => _registrationNumber; private set => _registrationNumber = value; }
        public string Color { get => _color; set => _color = value; }
        public int NumberOfWheels { get => _numberOfWheels; private set => _numberOfWheels = value; }

        public Vehicle(string registrationNumber, string color, int numberOfWheels)
        {
            _registrationNumber = registrationNumber;
            _color = color;
            _numberOfWheels = numberOfWheels;
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public string Specs()
        {
            string str = $"{RegistrationNumber} {GetType().Name} Color:{Color} {NumberOfWheels}";
            return str;
        }
    }
}

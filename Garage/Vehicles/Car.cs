using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Vehicles
{
    internal class Car : Vehicle
    {
        public string Fuel { get; private set; }

        public Car(string registrationNumber, string color, int numberOfWheels, string fuel)
            : base(registrationNumber, color, numberOfWheels) => Fuel = fuel;
    }
}

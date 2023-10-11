using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Vehicles
{
    internal class Bus : Vehicle
    {
        public int NumberOfSeats { get; private set; }
        public Bus(string registrationNumber, string color, int numberOfWheels, int numberOfSeats)
            : base(registrationNumber, color, numberOfWheels) => NumberOfSeats = numberOfSeats;
    }
}

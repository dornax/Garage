using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Vehicles
{
    internal class Boat : Vehicle
    {
        public float Length { get; private set; }
        public Boat(string registrationNumber, string color, int numberOfWheels, float length)
            : base(registrationNumber, color, numberOfWheels) => Length = length;
    }
}

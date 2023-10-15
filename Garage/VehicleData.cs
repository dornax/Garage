namespace Garage
{
    internal class VehicleData
    {
        public string Vehicle { get; internal set; } = default!;
        public string RegNum { get; internal set; } = default!;
        public string Color { get; internal set; } = default!;
        public int Wheels  { get; internal set; }
        public string Fuel { get; internal set; } = default!;
        public int CylinderVol { get; internal set; }
        public int Engines { get; internal set; }
        public int Seats { get; internal set; }
        public float Length { get; internal set; }

        public readonly IEnumerable<string> VList = new List<string> { { "Airplane" }, { "Boat" }, { "Bus" }, { "Car" }, { "Motorcycle" } };
        public readonly IEnumerable<string> FuelList = new List<string> { { "Diesel" }, { "Gasoline" }, { "Electric" } };
    }

}

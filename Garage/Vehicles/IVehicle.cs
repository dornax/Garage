namespace Garage.Vehicles
{
    internal interface IVehicle
    {
        string Color { get; set; }
        int NumberOfWheels { get; }
        string RegistrationNumber { get; }
        string ToString();
        string Specs();
    }
}
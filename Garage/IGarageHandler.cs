using Garage.Vehicles;

namespace Garage
{
    internal interface IGarageHandler
    {
        int Count { get; }
        bool IsFull { get; }
        int SizeOfGarage { get; }
        bool Add(IVehicle vehicle);
        string CountAllVehicles();
        string FileToSave();
        string Find(string registrationNumber);
        string GetAll(Func<IVehicle, bool> search);
        string GetAll();
        string GetAll(string vehicle);
        string GetAll(string vehicle, int numberOfWheels);
        string GetAll(string vehicle, string color, int wheel);
        string GetByColor(string color);
        string GetByColor(string color, int wheel);
        string GetByColor(string vehicle, string color);
        bool Remove(string registrationNumber);
    }
}
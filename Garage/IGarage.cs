using Garage.Vehicles;

namespace Garage
{
    internal interface IGarage<T> : IEnumerable<T> where T : IVehicle
    {
        bool IsFull { get; }
        int SizeOfGarage { get; }
        int Count { get; }
        bool Add(T item);
        bool Remove(T item);
    }
}
using Garage.Vehicles;
using System.Collections;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Tests")]

namespace Garage
{
    internal class Garage<T> : IGarage<T> where T : IVehicle
    {

        private T[] vehicles;
        private int _count = 0;
        public int SizeOfGarage { get; }
        public bool IsFull => _count == SizeOfGarage;
        public int Count => _count;

        public Garage(int numberOfVehicles)
        {
            SizeOfGarage = numberOfVehicles;
            vehicles = new T[SizeOfGarage];
        }

        public bool Add(T item)
        {
            if (_count < SizeOfGarage)
            {
                for (int i = 0; i < SizeOfGarage; i++)
                {
                    if (vehicles[i] is null)
                    {
                        vehicles[i] = item;
                        _count++;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Remove(T item)
        {
            int index;
            index = Array.IndexOf(vehicles, item);

            if (index != -1)
            {
                vehicles[index] = default!;
                _count--;
                return true;
            }
            else
                return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < SizeOfGarage; i++)
            {
                if (vehicles[i] != null)
                    yield return vehicles[i];
            }

        }
    }
}

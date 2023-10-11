using Garage;
using Garage.Vehicles;
using System.Linq.Expressions;

namespace Garage
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IUI ui = new ConsoleUI();
            Manager manager = new(ui);
            manager.Run();

        }
    }
}




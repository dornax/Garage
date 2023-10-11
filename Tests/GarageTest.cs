using Garage.Vehicles;

namespace Tests
{
    public class GarageTest
    {
        Garage<IVehicle> garage = default!;
        [Fact]
        public void Garage_Add_Count()
        {
            //Arrange
            garage = new(2);
            Car car1 = new Car("AAA111", "Blue", 4, "Gasoline");
            //Act    
            garage.Add(car1);

            //Assert
            int expectedCount = 1;
            Assert.Equal(expectedCount, garage.Count);
        }

        [Fact]
        public void Garage_IsFull()
        {
            garage = new(2);
            Car car1 = new Car("AAA111", "Blue", 4, "Gasoline");
            Car car2 = new Car("AAA112", "Blue", 4, "Gasoline");

            garage.Add(car1);
            garage.Add(car2);

            Assert.True(garage.IsFull);
        }

        [Fact]
        public void Garage_Remove_Count()
        {
            garage = new(2);
            Car car1 = new Car("AAA111", "Blue", 4, "Gasoline");
            Car car2 = new Car("AAA112", "Blue", 4, "Gasoline");

            garage.Add(car1);
            garage.Add(car2);
            garage.Remove(car2);

            int expectedCount = 1;
            Assert.Equal(expectedCount, garage.Count);
        }

    }
}
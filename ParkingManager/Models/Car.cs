
namespace ParkingSystem.Models
{
    public class Car: IVehicle
    {
        public Car()
        {
            this.WeightClass = WeightClass.Light;
        }
        public WeightClass WeightClass { get; private set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}

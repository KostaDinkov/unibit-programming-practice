namespace ParkingSystem.Models
{
    public class Truck : IVehicle
    {
        public Truck()
        {
            this.WeightClass = WeightClass.Heavy;
        }
        public WeightClass WeightClass { get; private set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
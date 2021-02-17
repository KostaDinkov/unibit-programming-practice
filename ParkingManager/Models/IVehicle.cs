namespace ParkingSystem.Models
{
    public interface IVehicle
    {
        public WeightClass WeightClass { get; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
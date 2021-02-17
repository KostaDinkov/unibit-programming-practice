using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem.Models
{
    public class Parking:IParking
    {
        private readonly int busCapacity;
        private int busSpacesOccupied;
        private readonly int carCapacity;
        private int carSpacesOccupied;
        private readonly int truckCapacity;
        private int truckSpacesOccupied;
        private ICollection<IVehicle> vehicles;

        public Parking(string name, int carCapacity, int busCapacity, int truckCapacity)
        {
            this.carCapacity = carCapacity;
            this.busCapacity = busCapacity;
            this.truckCapacity = truckCapacity;
            
            this.vehicles = new List<IVehicle>();
        }

        public bool ParkVehicle(IVehicle vehicle)
        {
            switch (vehicle)
            {
                case Car car:
                    if (this.carSpacesOccupied == this.carCapacity) return false;
                    this.carSpacesOccupied++;
                    break;
                case Bus bus:
                    if (this.busSpacesOccupied == this.busCapacity) return false;
                    this.busSpacesOccupied++;
                    break;
                case Truck truck:
                    if (this.truckSpacesOccupied ==this.truckCapacity) return false;
                    this.truckSpacesOccupied++;
                    break;
                default:
                    throw new NotImplementedException();
            }

            this.vehicles.Add(vehicle);
            return true;
        }

        public List<IVehicle> GetVehicles()
        {
            return this.vehicles.OrderBy(v=>v.WeightClass).ToList();
        }

        public int CarCapacity => this.carCapacity;
        public int BusCapacity => this.busCapacity;
        public int TruckCapacity => this.truckCapacity;
        public int CarSpacesOccupied => this.carSpacesOccupied;
        public int BusSpacesOccupied => this.busSpacesOccupied;
        public int TruckSpacesOccupied => this.truckSpacesOccupied;


        public string Name { get; set; }
    }
}
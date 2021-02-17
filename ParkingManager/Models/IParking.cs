using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingSystem.Models
{
    public interface IParking
    {
        public bool ParkVehicle(IVehicle vehicle);
        public List<IVehicle> GetVehicles();

        public int CarCapacity { get;  }
        public int BusCapacity { get; }
        public int TruckCapacity { get; }

        public int CarSpacesOccupied { get; }
        public int BusSpacesOccupied { get; }
        public int TruckSpacesOccupied { get; }
    }
}

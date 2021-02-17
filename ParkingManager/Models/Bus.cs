using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingSystem.Models
{
    public class Bus:IVehicle
    {
        public Bus()
        {
            this.WeightClass = WeightClass.Medium;
        }
        public WeightClass WeightClass { get; private set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}

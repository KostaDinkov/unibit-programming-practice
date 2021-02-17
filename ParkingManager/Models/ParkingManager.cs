using System.Collections.Generic;
using System.Text;
using ParkingSystem.Common;

namespace ParkingSystem.Models
{
    public class ParkingManager
    {
        private readonly IDictionary<string, Parking> parkings;

        public ParkingManager()
        {
            this.parkings = new Dictionary<string, Parking>();
        }

        public bool ParkVehicle(IVehicle vehicle)
        {
            foreach (var (_, parking) in this.parkings)
            {
                if (parking.ParkVehicle(vehicle))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddParking(string name, int carCap, int busCap, int truckCap)
        {
            if (!this.parkings.ContainsKey(name))
            {
                this.parkings.Add(name, new Parking(name, carCap, busCap, truckCap));
            }
        }

        public string PrintParkedVehicles(string parkingName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(Messages.ParkedVehiclesMsg, parkingName));
            foreach (var vehicle in this.parkings[parkingName].GetVehicles())
            {
                sb.AppendLine(string.Format(Messages.MakeModelMsg, vehicle.Make, vehicle.Model));
            }

            return sb.ToString().Trim('\r', '\n');
        }

        public string PrintParkingStatus()
        {
            var sb = new StringBuilder();
            foreach (var (name, parking) in this.parkings)
            {
                sb.AppendLine(string.Format(Messages.ParkingStatusMsg, name));
                sb.AppendLine(string.Format(Messages.LightVehiclesMsg, parking.CarCapacity, parking.CarSpacesOccupied));
                sb.AppendLine(string.Format(Messages.MediumVehiclesMsg, parking.BusCapacity,
                    parking.BusSpacesOccupied));
                sb.AppendLine(string.Format(Messages.HeavyVehiclesMsg, parking.TruckCapacity,
                    parking.TruckSpacesOccupied));
            }

            return sb.ToString().Trim('\r', '\n');
        }
    }
}
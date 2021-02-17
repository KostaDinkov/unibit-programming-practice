using System;
using ParkingSystem.Common;

namespace ParkingSystem.Models
{
    internal class ConsoleTerminal : ITerminal
    {
        private bool isRunning;
        private readonly ParkingManager parkingManager;

        public ConsoleTerminal(ParkingManager parkingManager)
        {
            this.parkingManager = parkingManager;
            this.isRunning = false;
        }

        public void Start()
        {
            this.isRunning = true;
            while (this.isRunning)
            {
                this.ReadCommand();
            }
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string message, params string[] parameters)
        {
            Console.WriteLine(message, parameters);
        }

        private void ReadCommand()
        {
            var commandParams = this.ReadLine().Split(" ");
            var command = commandParams[0];
            IVehicle vehicle = new Car();
            switch (command)
            {
                case "Паркинг":
                {
                    var parkingName = commandParams[1];
                    var carCap = int.Parse(commandParams[2]);
                    var busCap = int.Parse(commandParams[3]);
                    var truckCap = int.Parse(commandParams[4]);
                    this.parkingManager.AddParking(parkingName, carCap, busCap, truckCap);
                }
                    break;
                case "Кола":
                case "Бус":
                case "Камион":
                    vehicle = command switch
                    {
                        "Кола" => new Car(),
                        "Бус" => new Bus(),
                        "Камион" => new Truck(),
                    };
                    var make = commandParams[1];
                    var model = commandParams[2];
                    vehicle.Make = make;
                    vehicle.Model = model;

                    if (!this.parkingManager.ParkVehicle(vehicle))
                        this.WriteLine(Messages.NoSpacesAvailableMsg, vehicle.Make, vehicle.Model);

                    break;
                case "Печат":
                {
                    var parkingName = commandParams[1];
                    this.WriteLine(this.parkingManager.PrintParkedVehicles(parkingName));
                    break;
                }
                case "Край":
                    this.WriteLine(this.parkingManager.PrintParkingStatus());
                    this.isRunning = false;
                    break;
            }
        }
    }
}
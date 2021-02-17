using ParkingSystem.Models;

namespace ParkingSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parkingManager = new ParkingManager();
            var terminal = new ConsoleTerminal(parkingManager);
            terminal.Start();
        }
    }
}
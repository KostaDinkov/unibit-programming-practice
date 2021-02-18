using System;
using Restaurant.Models;

namespace Restaurant
{
    class Program
    {
        static void Main(string[] args)
        {
            RestrauntManager manager = new RestrauntManager();
            ITerminal terminal = new ConsoleTerminal(manager);
            terminal.Start();
        }
    }
}

using System;

namespace Restaurant.Exceptions
{
    internal class CommandFormatException : Exception
    {
        public CommandFormatException(string message) : base(message)
        {
        }
    }
}
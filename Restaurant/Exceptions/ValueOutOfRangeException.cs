using System;

namespace Restaurant.Exceptions
{
    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException(string message): base(message)
        {
            
        }
    }
}
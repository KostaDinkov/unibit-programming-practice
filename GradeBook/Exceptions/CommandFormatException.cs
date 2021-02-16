using System;

namespace GradeBook.Exceptions
{
    public class CommandFormatException : Exception
    {
        public CommandFormatException():base("Command is not in the specified format")
        {
            
        }

        public CommandFormatException(string msg) : base(msg)
        {

        }
    }
}
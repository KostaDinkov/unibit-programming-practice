using System;

namespace GradeBook.Exceptions
{
    public class CommandFormatException : Exception
    {
        public CommandFormatException()
        {

        }

        public CommandFormatException(string msg) : base(msg)
        {

        }
    }
}
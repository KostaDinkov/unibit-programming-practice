using System;

namespace GradeBook
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
using System;

namespace GradeBook
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }
        public NotFoundException(string msg):base(msg)
        {
        }
    }
}
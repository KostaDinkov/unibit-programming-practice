using GradeBook.Models;

namespace GradeBook
{
    class Program
    {
        public const string CommandInfoFilePath = "command-help.json";
        static void Main()
        {
            var school = new School("Unibit");

            school.StartTerminal();

        }
    }
}
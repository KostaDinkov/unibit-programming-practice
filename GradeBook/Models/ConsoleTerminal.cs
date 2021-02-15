using System;
using GradeBook.Exceptions;
using GradeBook.Utils;

namespace GradeBook.Models
{
    public class ConsoleTerminal : ITerminal
    {
        private School school;

        public ConsoleTerminal(School school)
        {
            this.school = school;
            this.IsRunning = true;
        }

        public bool IsRunning { get; set; }

        public void ReadCommand()
        {
            Console.Write(">>>");
            var commandParts = Console.ReadLine().Split(":");
            var command = commandParts[0].Trim();

            try
            {
                switch (command)
                {
                    case ("add-student"):
                        var student = Validate.AddStudent(commandParts);
                        this.school.AddStudent(student);
                        this.Log($"Student {student.FullName} added to School");
                        break;

                    case ("add-course"):
                        var course = Validate.AddCourse(commandParts);
                        this.school.AddCourse(course);
                        this.Log($"Course {course.Name} added to School");
                        break;

                    case "add-grade":
                        Validate.AddGrade(commandParts, this.school);
                        break;

                    case "get-grades":
                        string result = Validate.GetGrades(commandParts, this.school);
                        this.Log(result);
                        break;

                    case "get-semester-stats":
                        result = Validate.GetSemesterStats(commandParts, this.school);
                        this.Log(result);
                        break;

                    case "students":
                        this.Log(this.school.GetStudentsString());
                        break;
                    
                    case "help":
                        this.Log(this.school.GetCommandHelp());
                        break;
                    
                    default:
                        this.Log("Command Not Recognized. Please enter a valid command or enter h for help");
                        break;
                }
            }
            catch (CommandFormatException e)
            {
                this.Log(e.Message);
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
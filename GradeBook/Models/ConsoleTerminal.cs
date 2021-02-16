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
            

            try
            {
                var commandLine = Console.ReadLine().Split(":");
                
                var command = commandLine[0].Trim();
                string result;
                switch (command)
                {
                    case ("add-student"):
                        var student = this.school.Validator.ValidateAddStudent(command, commandLine);
                        this.school.AddStudent(student);
                        this.Log($"Student {student.FullName} added to School");
                        break;

                    case ("add-course"):
                        var course = this.school.Validator.ValidateAddCourse(command, commandLine);
                        this.school.AddCourse(course);
                        this.Log($"Course {course.Name} added to School");
                        break;
                    
                    case "get-courses":
                        result = this.school.GetCoursesString();
                        this.Log(result);
                        break;

                    case "add-grade":
                        this.school.Validator.ValidateAddGrade(command, commandLine);
                        break;

                    case "add-grades-bulk":
                        var data = Console.ReadLine();
                        var studentGrades = this.school.Validator.ValidateAddGradesBulk(commandLine[1].Trim(), data);
                        this.school.AddGradesBulk(studentGrades.Item1.FullName, studentGrades.Item2);
                        break;


                    case "get-grades":
                        result = this.school.Validator.ValidateGetGrades(commandLine[1].Trim());
                        this.Log(result);
                        break;

                    case "get-semester-stats":
                        result = this.school.Validator.ValidateGetSemesterStats(commandLine[1].Trim());
                        this.Log(result);
                        break;

                    case "get-students":
                        this.Log(this.school.GetStudentsString());
                        break;
                    
                    case "h":
                        this.Log(this.school.GetCommandHelp());
                        break;
                    
                    default:
                        this.Log("Command Not Recognized. Please enter a valid command or enter h for help");
                        break;
                }
            }
            catch (Exception e) when(e is CommandFormatException || e is NotFoundException)
            {
                this.Log(e.Message);
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }


        public void Start()
        {
            this.Log($"{this.school.Name} learning management system.");
            this.Log("For a list of available commands type h: ");
            this.Log("Enter Command:");

            while (this.IsRunning)
            {
                this.ReadCommand();
            }
        }
    }
}
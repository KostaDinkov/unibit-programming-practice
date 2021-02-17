using System;
using GradeBook.Common;
using GradeBook.Exceptions;

namespace GradeBook.Models
{
    public class ConsoleTerminal : ITerminal
    {
        private readonly School school;

        public ConsoleTerminal(School school)
        {
            this.school = school;
            this.IsRunning = true;
        }

        public bool IsRunning { get; set; }

        public void ReadCommand()
        {
            try
            {
                var commandLine = Console.ReadLine().Split(":");

                var command = commandLine[0].Trim();
                string result;
                switch (command)
                {
                    case "add-student":
                        var student = this.school.Validator.ValidateAddStudent(command, commandLine);
                        this.school.AddStudent(student);
                        this.Log(Messages.StudentAddedMsg, student.FullName);
                        break;

                    case "add-course":
                        var course = this.school.Validator.ValidateAddCourse(command, commandLine);
                        this.school.AddCourse(course);
                        this.Log(Messages.CourseAddedMsg, course.Name);
                        break;

                    case "get-courses":
                        result = this.school.GetCoursesString();
                        this.Log(result);
                        break;

                    case "add-grade":
                        this.school.Validator.ValidateAddGrade(command, commandLine);
                        break;

                    case "add-grades-bulk":
                        var data = Console.ReadLine().Split(";");
                        var studentGrades = this.school.Validator.ValidateAddGradesBulk(command, commandLine, data);
                        this.school.AddGradesBulk(studentGrades.Item1.FullName, studentGrades.Item2);
                        break;

                    case "get-grades":
                        result = this.school.Validator.ValidateGetGrades(commandLine[1].Trim());
                        this.Log(result);
                        break;

                    case "get-semester-stats":
                        result = this.school.Validator.ValidateGetSemesterStats(command, commandLine);
                        this.Log(result);
                        break;

                    case "get-students":
                        this.Log(this.school.GetStudentsString());
                        break;

                    case "h":
                        this.Log(this.school.GetCommandHelp());
                        break;
                    case "exit":
                        this.IsRunning = false;
                        break;
                    default:
                        this.Log(Messages.CommandNotRecognizedMsg);
                        break;
                }
            }
            catch (Exception e) when (e is CommandFormatException || e is NotFoundException)
            {
                this.Log(e.Message);
            }
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, params string[] msgParams)
        {

            Console.WriteLine(message, msgParams);
        }


        public void Start()
        {
            Console.WriteLine(Messages.WelcomeMsg, this.school.Name);
            while (this.IsRunning)
            {
                this.ReadCommand();
            }
        }
    }
}
using System;
using System.Globalization;
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

                switch (command)
                {
                    case "add-student":
                    {
                        var studentName = this.school.Validator.ValidateAddStudent(command, commandLine);
                        this.school.AddStudent(new Student {FullName = studentName});
                        this.Log(Messages.StudentAddedMsg, studentName);
                        break;
                    }
                    case "add-course":
                    {
                        var (semester, courseName, lectureHours, practiceHours, teacherName) =
                            this.school.Validator.ValidateAddCourse(command, commandLine);
                        var course = new Course
                        {
                            Name = courseName,
                            LectureCount = lectureHours,
                            PracticeCount = practiceHours,
                            TeacherName = teacherName,
                            Semester = semester
                        };
                        this.school.AddCourse(course);
                        this.Log(Messages.CourseAddedMsg, course.Name);
                        break;
                    }
                    case "get-courses":
                    {
                        var result = this.school.GetCoursesString();
                        this.Log(result);
                        break;
                    }
                    case "add-grade":
                    {
                        var (studentName, courseName, grade) =
                            this.school.Validator.ValidateAddGrade(command, commandLine);
                        this.school.AddGrade(studentName, courseName, grade);
                        this.Log(Messages.GradeAddedMsg, courseName, grade.ToString(CultureInfo.InvariantCulture),
                            studentName);
                        break;
                    }
                    case "add-grades-bulk":
                    {
                        var data = Console.ReadLine().Split(";");
                        var (studentName, courseInfos) =
                            this.school.Validator.ValidateAddGradesBulk(command, commandLine, data);

                        this.school.AddGradesBulk(studentName, courseInfos);
                        this.Log(Messages.GradesAdded);
                        break;
                    }
                    case "get-grades":
                    {
                        var studentName = this.school.Validator.ValidateGetGrades(command, commandLine);
                        var result = this.school.GetGradesString(studentName);
                        this.Log(result);
                        break;
                    }
                    case "get-semester-stats":
                    {
                        var studentName = this.school.Validator.ValidateGetSemesterStats(command, commandLine);
                        var result = this.school.GetSemesterStats(studentName);
                        this.Log(result);
                        break;
                    }
                    case "get-students":
                    {
                        this.Log(this.school.GetStudentsString());
                        break;
                    }
                    case "h":
                    {
                        this.Log(this.school.GetCommandHelp());
                        break;
                    }
                    case "exit":
                    {
                        this.IsRunning = false;
                        break;
                    }
                    default:
                    {
                        this.Log(Messages.CommandNotRecognizedMsg);
                        break;
                    }
                }
            }
            catch (Exception e) when (e is CommandFormatException || e is NotFoundException ||
                                      e is EntityExistsException)
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
            this.Log(Messages.WelcomeMsg, this.school.Name);
            while (this.IsRunning)
            {
                this.ReadCommand();
            }
        }

        public void Log(string message, params string[] msgParams)
        {
            Console.WriteLine(message, msgParams);
        }
    }
}
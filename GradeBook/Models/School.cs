using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using GradeBook.Common;
using GradeBook.Exceptions;

namespace GradeBook.Models
{
    public class School
    {
        private readonly ITerminal terminal;
        public List<CommandInfo> CommandInfos { get; private set; }

        public School(string name)
        {
            this.Name = name;
            this.Students = new List<Student>();
            this.Courses = new List<Course>();
            this.terminal = new ConsoleTerminal(this);
            this.CommandInfos = new List<CommandInfo>();
            this.Validator = new CommandValidator(this);
            this.ReadCommandInfo();
        }

        public CommandValidator Validator { get; }

        public List<Course> Courses { get; }

        public List<Student> Students { get; }


        public string Name { get; }

        public void AddStudent(Student student)
        {
            if (this.Students.Any(s => s.FullName == student.FullName))
            {
                return;
            }
            this.Students.Add(student);
        }


        public void AddCourse(Course course)
        {
            this.Courses.Add(course);
        }

        public string GetCoursesString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(Messages.ListOfAllCoursesMsg,this.Name));
            var counter = 1;

            foreach (var course in this.Courses.OrderBy(c => c.Semester).ThenBy(c=>c.Name))
            {
                sb.AppendLine($"  {counter}. {course.Name}, {course.TeacherName}");
                counter++;
            }

            sb.AppendLine(string.Format(Messages.TotalCoursesMsg, this.Courses.Count));

            return sb.ToString().Trim('\r', '\n');
        }

        public void AddGrade(string name, string courseName, double grade)
        {
            var student = this.Students.FirstOrDefault(s => s.FullName == name);
            if (student != null)
            {
                student.AddGrade(courseName, grade);
            }
            else
            {
                throw new NotFoundException(Messages.StudentNotFoundMsg);
            }
        }

        public void AddGradesBulk(string name, Dictionary<string, double> courseGrades)
        {
            var student = this.Students.FirstOrDefault(s => s.FullName == name) ?? new Student {FullName = name};
            foreach (var courseGrade in courseGrades)
            {
                student.AddGrade(courseGrade.Key, courseGrade.Value);
            }
        }

        public string GetGradesString(string name)
        {
            var sb = new StringBuilder();

            var student = this.Students.FirstOrDefault(s => s.FullName == name);

            if (student != null)
            {
                var result = student.CoursesGrades.Select(cg =>
                {
                    var course = this.Courses.FirstOrDefault(c => c.Name == cg.Key);

                    return new
                    {
                        course.Semester,
                        course.Name,
                        course.TeacherName,
                        Grade = cg.Value
                    };
                }).OrderBy(c => c.Semester);

                sb.AppendLine(string.Format(Messages.GradesForMsg, name));

                foreach (var entry in result)
                {
                    sb.AppendLine(
                        string.Format(Messages.GradesLineMsg,entry.Semester, entry.Name, entry.TeacherName, entry.Grade));
                }
            }

            return sb.ToString();
        }

        public string GetSemesterStats(string studentName)
        {
            var student = this.Students.FirstOrDefault(s => s.FullName == studentName);
            
            var courseCount = student.CoursesGrades.Count;

            var result = student.CoursesGrades.Join(this.Courses, kvp => kvp.Key, course => course.Name,
                (kvp, course) => new
                {
                    course.Name,
                    TotalHours = course.LectureCount + course.PracticeCount,
                    course.Semester,
                    Grade = kvp.Value
                }).GroupBy(c => c.Semester,
                (key, value) => new
                {
                    Semester = key, TotalHours = value.Sum(c => c.TotalHours),
                    AvgGrade = value.Average(c => c.Grade)
                }).OrderBy(s => s.Semester);
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(Messages.SemesterStatsMsg, student.FullName, courseCount));
            var counter = 1;
            foreach (var entry in result)
            {
                sb.AppendLine(string.Format(Messages.SemesterStatsLineMs,counter, entry.Semester, entry.TotalHours, entry.AvgGrade));
                counter++;
            }

            sb.AppendLine(string.Format(Messages.TotalAverageGradeMsg, student.GetAverageGrade()));

            return sb.ToString();
        }

        public string GetStudentsString()
        {
            return string.Join("\n", this.Students);
        }

        public string GetCommandHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(Messages.AvailableCommands, this.Name) + new string('-', 20));

            foreach (var commandInfo in this.CommandInfos)
            {
                sb.AppendLine(commandInfo.ToString());
                sb.AppendLine(new string('-', 20));
            }

            return sb.ToString();
        }

        private void ReadCommandInfo()
        {
            var jsonInfo = File.ReadAllText(Program.CommandInfoFilePath);
            this.CommandInfos = JsonSerializer.Deserialize<List<CommandInfo>>(jsonInfo);
        }

        public void StartTerminal()
        {
            this.terminal.Start();
        }
    }
}
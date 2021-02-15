using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace GradeBook
{
    public class School
    {
        private List<CommandInfo> commandInfos;
        private readonly List<Course> courses;
        private readonly ITerminal terminal;

        public School(string name)
        {
            this.Name = name;
            this.Students = new List<Student>();
            this.courses = new List<Course>();
            this.terminal = new ConsoleTerminal(this);
            this.commandInfos = new List<CommandInfo>();
            this.ReadCommandInfo();
        }

        public List<Student> Students { get; }


        public string Name { get; }

        public void AddStudent(Student student)
        {
            this.Students.Add(student);
        }


        public void AddCourse(Course course)
        {
            this.courses.Add(course);
        }

        public void AddGrade(string name, string courseName, double grade)
        {
            var student = this.Students.FirstOrDefault(s => s.FullName == name);
            if (student != null)
            {
                student.AddGrade(courseName, grade);
            }

            //todo throw exception
        }

        public string GetGradesString(string name)
        {
            var sb = new StringBuilder();

            var student = this.Students.FirstOrDefault(s => s.FullName == name);

            if (student != null)
            {
                var result = student.CoursesGrades.Select(c =>
                {
                    var course = this.courses.FirstOrDefault(course => course.Name == c.Key);

                    return new
                    {
                        course.Semester,
                        course.Name,
                        course.TeacherName,
                        Grade = c.Value
                    };
                }).OrderBy(c => c.Semester);

                sb.AppendLine($"Grades for student {name}:");

                foreach (var entry in result)
                {
                    sb.AppendLine(
                        $"Semester: {entry.Semester}, Course: {entry.Name}, Teacher: {entry.TeacherName}, Grade: {entry.Grade}");
                }
            }

            return sb.ToString();
        }

        public string GetSemesterStats(string studentName)
        {
            var student = this.Students.FirstOrDefault(s => s.FullName == studentName);
            if (student != null)
            {
                var courseCount = student.CoursesGrades.Count;

                var result = student.CoursesGrades.Join(this.courses, kvp => kvp.Key, course => course.Name,
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
                    }).OrderBy(s=>s.Semester);
                var sb = new StringBuilder();
                sb.AppendLine($"Student: {student.FullName}");
                sb.AppendLine($"Total courses: {courseCount}");
                sb.AppendLine("Semester average grades and total study hours:");
                int counter = 1;
                foreach (var entry in result)
                {
                    sb.AppendLine($"\t{counter}. Semester {entry.Semester}, {entry.TotalHours}: {entry.AvgGrade}");
                    counter++;
                }

                return sb.ToString();
            }
            else
            {
                return "";
            }

            

            //todo throw exception
        }

        public string GetStudentsString()
        {
            return string.Join("\n", this.Students);
        }

        public string GetCommandHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{this.Name} - available commands:\n" + new string('-', 20));

            foreach (var commandInfo in this.commandInfos)
            {
                sb.AppendLine(commandInfo.ToString());
                sb.AppendLine(new string('-', 20));
            }

            return sb.ToString();
        }

        private void ReadCommandInfo()
        {
            var jsonInfo = File.ReadAllText(Program.CommandInfoFilePath);
            this.commandInfos = JsonSerializer.Deserialize<List<CommandInfo>>(jsonInfo);
        }

        public void StartTerminal()
        {
            this.terminal.Log($"{this.Name} learning management system.");
            this.terminal.Log("For a list of available commands type help: ");
            this.terminal.Log("Enter Command:");

            while (this.terminal.IsRunning)
            {
                this.terminal.ReadCommand();
            }
        }
    }
}
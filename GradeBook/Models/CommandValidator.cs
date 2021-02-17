using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using GradeBook.Exceptions;

namespace GradeBook.Models
{
    public class CommandValidator
    {
        private readonly School school;

        public CommandValidator(School school)
        {
            this.school = school;
        }

        public string ValidateAddStudent(string command, string[] commandLine)
        {
            if (!this.IsCorrectLen(commandLine, 2)) throw new CommandFormatException(this.GetCommandFormat(command));

            var studentName = commandLine[1].Trim();
            if (!Regex.IsMatch(studentName, "^[a-zA-Z ]*$") ||
                string.IsNullOrWhiteSpace(studentName))
                throw new CommandFormatException(this.GetCommandFormat(command));

            return studentName;
        }

        public (int semester, string courseName, int lectureHours, int practiceHours, string teacherName)
            ValidateAddCourse(string command, string[] parameters)
        {
            if (!this.IsCorrectLen(parameters, 2)) throw new CommandFormatException(this.GetCommandFormat(command));
            var courseDataParts = parameters[1].Split(",").Select(p => p.Trim()).ToArray();

            if (!this.IsCorrectLen(courseDataParts, 5))
                throw new CommandFormatException(this.GetCommandFormat(command));

            var courseName = courseDataParts[1];
            var teacherName = courseDataParts[4];
            if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(teacherName))
                throw new CommandFormatException(this.GetCommandFormat(command));

            try
            {
                var semester = int.Parse(courseDataParts[0]);
                var lectureCount = int.Parse(courseDataParts[2]);
                var practiceCount = int.Parse(courseDataParts[3]);

                return (semester, courseName, lectureCount, practiceCount, teacherName);
            }
            catch (Exception e) when (e is FormatException || e is ArgumentNullException || e is OverflowException)
            {
                throw new CommandFormatException(this.GetCommandFormat(command));
            }
        }

        public (string studentName, string courseName, double grade) ValidateAddGrade(string command,
            string[] parameters)
        {
            if (!this.IsCorrectLen(parameters, 2))
                throw new CommandFormatException(this.GetCommandFormat(command));

            var dataParts = parameters[1].Split(",");
            if (!this.IsCorrectLen(dataParts, 3)) throw new CommandFormatException(this.GetCommandFormat(command));

            var studentName = dataParts[0].Trim();
            if (string.IsNullOrWhiteSpace(studentName))
                throw new CommandFormatException(this.GetCommandFormat(command));

            var courseName = dataParts[1].Trim();
            if (string.IsNullOrWhiteSpace(courseName)) throw new CommandFormatException(this.GetCommandFormat(command));

            double grade = 0;
            try
            {
                grade = double.Parse(dataParts[2], CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                throw new CommandFormatException(this.GetCommandFormat(command));
            }

            return (studentName, courseName, grade);
        }

        public string ValidateGetSemesterStats(string command, string[] parameters)
        {
            if (!this.IsCorrectLen(parameters, 2))
                throw new CommandFormatException(this.GetCommandFormat(command));

            var studentName = parameters[1].Trim();
            if (string.IsNullOrWhiteSpace(studentName))
                throw new CommandFormatException(this.GetCommandFormat(command));

            return studentName;
        }

        private bool IsCorrectLen(string[] commandLine, int partCount)
        {
            return commandLine.Length == partCount;
        }

        public string ValidateGetGrades(string command, string[] parameters)
        {
            if (!this.IsCorrectLen(parameters, 2)) throw new CommandFormatException(this.GetCommandFormat(command));
            var studentName = parameters[1].Trim();

            if (string.IsNullOrWhiteSpace(studentName))
                throw new CommandFormatException(this.GetCommandFormat(command));
            return studentName;
        }

        public (string Student, List<CourseInfo> CourseInfos)
            ValidateAddGradesBulk(string command, string[] commandLine, string[] data)
        {
            if (!this.IsCorrectLen(commandLine, 2)) throw new CommandFormatException(this.GetCommandFormat(command));

            var studentName = commandLine[1].Trim();

            var courseInfos = new List<CourseInfo>();

            foreach (var courseGrade in data)
            {
                var parameters = courseGrade.Split(",").Select(s => s.Trim()).ToArray();
                if (!this.IsCorrectLen(parameters, 6)) throw new CommandFormatException(this.GetCommandFormat(command));

                try
                {
                    var semester = int.Parse(parameters[0]);
                    var courseName = parameters[1];
                    var lectureHours = int.Parse(parameters[2]);
                    var exerciseHours = int.Parse(parameters[3]);
                    var teacherName = parameters[4];
                    var grade = double.Parse(parameters[5], CultureInfo.InvariantCulture);

                    if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(teacherName))
                        throw new CommandFormatException(this.GetCommandFormat(command));

                    courseInfos.Add(new CourseInfo
                    {
                        Semester = semester,
                        Name = courseName,
                        LectureCount = lectureHours,
                        PracticeCount = exerciseHours,
                        Grade = grade,
                        TeacherName = teacherName,
                    });
                }
                catch (Exception e)
                {
                    throw new CommandFormatException(this.GetCommandFormat(command));
                }
            }

            return (studentName, courseInfos);
        }

        private string GetCommandFormat(string command)
        {
            return this.school.CommandInfos.FirstOrDefault(ci => ci.Name == command)?.Format ?? "";
        }
    }
}
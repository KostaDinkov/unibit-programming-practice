using System;
using System.Globalization;
using System.Linq;
using GradeBook.Exceptions;
using GradeBook.Models;

namespace GradeBook.Utils
{
    public class Validate
    {
        public static Student AddStudent(string[] commandParts)
        {
            if (commandParts.Length < 2 || string.IsNullOrWhiteSpace(commandParts[1]))
            {
                throw new CommandFormatException("The command is not in the specified format.\nadd-student: {studentName}");
            }
            else
            {
                var student = new Student();
                student.FullName = commandParts[1].Trim();
                return student;
            }
        }

        public static Course AddCourse(string[] commandParts)
        {
            if (commandParts.Length < 2 || string.IsNullOrWhiteSpace(commandParts[1]))
            {
                throw new CommandFormatException("The command is not in the specified format.\nadd-course: {semester}, {courseName}, {lectureHours}, {practiceHours}, {teacherName}");
            }

            var parameters = commandParts[1].Split(",");
            if (parameters.Length != 5)
            {
                throw new CommandFormatException("The command is not in the specified format.\n{semester}, {courseName}, {lectureHours}, {practiceHours}, {teacherName}");
            }

            var course = new Course
            {
                Name = parameters[1].Trim(),
                TeacherName = parameters[4]
            };

            try
            {
                course.Semester = int.Parse(parameters[0]);
                course.LectureCount = int.Parse(parameters[2]);
                course.PracticeCount = int.Parse(parameters[3]);
            }
            catch (Exception e) when (e is FormatException || e is ArgumentNullException || e is OverflowException)
            {
                throw new CommandFormatException("The command is not in the specified format.\n"+ e.Message);
            }

            return course;
        }

        public static void AddGrade(string[] commandParts, School school)
        {
            if (commandParts.Length != 2)
            {
                throw new CommandFormatException("The command is not in the specified format.\nadd-grade: {studentName} {courseName} {grade}");
            }

            var parameters = commandParts[1].Split(",");

            var studentName = parameters[0].Trim();
            var courseName = parameters[1].Trim();
            var grade = double.Parse(parameters[2], CultureInfo.InvariantCulture);
            
            var student = school.Students.FirstOrDefault(s => s.FullName == studentName);
            if (student == null)
            {
                throw new NotFoundException("The student could not be found");
            }

            student.CoursesGrades.Add(courseName,grade);

        }

        public static string GetSemesterStats(string[] commandParts, School school)
        {
            if (commandParts.Length != 2)
            {
                throw new CommandFormatException("The command is not in the specified format.\nadd-grade: {studentName} {courseName} {grade}");
            }
            var parameters = commandParts[1].Split(",");

            var studentName = parameters[0].Trim();
            var student = school.Students.FirstOrDefault(s => s.FullName == studentName);
            if (student == null)
            {
                throw new NotFoundException("The student could not be found");
            }

            return school.GetSemesterStats(studentName);

        }

        public static string GetGrades(string[] commandParts, School school)
        {
            if (commandParts.Length != 2)
            {
                throw new CommandFormatException("The command is not in the specified format.\nadd-grade: {studentName} {courseName} {grade}");
            }
            var parameters = commandParts[1].Split(",");
            var studentName = parameters[0].Trim();
            var student = school.Students.FirstOrDefault(s => s.FullName == studentName);
            if (student == null)
            {
                throw new NotFoundException("The student could not be found");
            }

            return school.GetGradesString(studentName);

        }
    }
}

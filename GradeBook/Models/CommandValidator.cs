using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using GradeBook.Common;
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

        public Student ValidateAddStudent(string command, string[] commandLine)
        {
            if (!this.IsCorrectLen(commandLine, 2)) this.ThrowFormatError(command);
            
            var studentName = commandLine[1].Trim();
            if(!Regex.IsMatch(studentName, "^[a-zA-Z ]*$") ||
             string.IsNullOrWhiteSpace(studentName)) this.ThrowFormatError(command);
            
            var student = new Student {FullName = studentName};
            return student;
        }

        public Course ValidateAddCourse(string command, string[] parameters)
        {
            if (!this.IsCorrectLen(parameters, 2)) this.ThrowFormatError(command);
            var courseDataParts = parameters[1].Split(",").Select(p => p.Trim()).ToArray();

            if (!this.IsCorrectLen(courseDataParts, 5)) this.ThrowFormatError(command);
            
            var course = new Course
            {
                Name = courseDataParts[1].Trim(),
                TeacherName = courseDataParts[4]
            };

            if (string.IsNullOrWhiteSpace(course.Name) || string.IsNullOrWhiteSpace(course.TeacherName)) this.ThrowFormatError(command);
            
            try
            {
                course.Semester = int.Parse(courseDataParts[0]);
                course.LectureCount = int.Parse(courseDataParts[2]);
                course.PracticeCount = int.Parse(courseDataParts[3]);
            }
            catch (Exception e) when (e is FormatException || e is ArgumentNullException || e is OverflowException)
            {
                this.ThrowFormatError(command);
            }

            return course;
        }

        public void ValidateAddGrade(string command, string[] parameters)
        {

            if(!this.IsCorrectLen(parameters, 2)) this.ThrowFormatError(command);

            var dataParts = parameters[1].Split(",");
            if (!this.IsCorrectLen(dataParts, 3)) this.ThrowFormatError(command);
            
            var studentName = dataParts[0].Trim();
            if(string.IsNullOrWhiteSpace(studentName)) this.ThrowFormatError(command);
            
            var courseName = dataParts[1].Trim();
            if(string.IsNullOrWhiteSpace(courseName)) this.ThrowFormatError(command);

            double grade = 0;
            try
            {
                grade = double.Parse(dataParts[2], CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                this.ThrowFormatError(command);
            }

            var student = this.school.Students.FirstOrDefault(s => s.FullName == studentName) ??
                          new Student {FullName = studentName};
            student.AddGrade(courseName, grade);
        }

        public string ValidateGetSemesterStats(string command, string[] parameters)
        {
            if(!this.IsCorrectLen(parameters,2)) this.ThrowFormatError(command);
            var studentName = parameters[1].Trim();
            var student = this.ValidateStudent(studentName);
            return this.school.GetSemesterStats(studentName);
        }

        public bool IsCorrectLen(string[] commandLine, int partCount)
        {
            return commandLine.Length == partCount;
        }

        public string ValidateGetGrades(string input)
        {
            var student = this.ValidateStudent(input);
            return this.school.GetGradesString(input);
        }

        public (Student, Dictionary<string, double>) ValidateAddGradesBulk(string command, string[] commandLine, string[] data)
        {
            if(!this.IsCorrectLen(commandLine,2)) this.ThrowFormatError(command);
            
            Student student;
            var studentName = commandLine[1].Trim();
            try
            {
                student = this.ValidateStudent(studentName);
            }
            catch (NotFoundException e)
            {
                student = new Student {FullName = studentName};
                this.school.AddStudent(student);
            }
            
            var result = new Dictionary<string, double>();

            foreach (var courseGrade in data)
            {
                var parameters = courseGrade.Split(",").Select(s => s.Trim()).ToArray();
                if(!this.IsCorrectLen(parameters,6)) this.ThrowFormatError(command);
                
                try
                {
                    var semester = int.Parse(parameters[0]);
                    var courseName = parameters[1];
                    var lectureHours = int.Parse(parameters[2]);
                    var exerciseHours = int.Parse(parameters[3]);
                    var teacherName = parameters[4];
                    var grade = double.Parse(parameters[5], CultureInfo.InvariantCulture);

                    if(string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(teacherName)) this.ThrowFormatError(command);

                    if (this.school.Courses.All(c => c.Name != courseName))
                    {
                        this.school.AddCourse(new Course
                        {
                            Semester = semester, Name = courseName, LectureCount = lectureHours,
                            PracticeCount = exerciseHours, TeacherName = teacherName
                        });
                    }

                    result.Add(courseName, grade);
                }
                catch (Exception e)
                {
                    this.ThrowFormatError(command);
                }
            }

            return (student, result);
        }


        private Student ValidateStudent(string studentName)
        {
            var student = this.school.Students.FirstOrDefault(s => s.FullName == studentName);
            if (student == null)
            {
                throw new NotFoundException(Messages.StudentNotFoundMsg);
            }

            return student;
        }

        private void ThrowFormatError(string command)
        {
            throw new CommandFormatException(this.school.CommandInfos.FirstOrDefault(ci => ci.Name == command)
                ?.Format ?? "");
        }
    }
}
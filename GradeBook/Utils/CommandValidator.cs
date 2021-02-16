using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GradeBook.Exceptions;
using GradeBook.Models;

namespace GradeBook.Utils
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
            if (string.IsNullOrWhiteSpace(studentName)) this.ThrowFormatError(command);
            
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
            if (this.IsCorrectLen(parameters,3)) this.ThrowFormatError(command);
            

            var studentName = parameters[0].Trim();
            var courseName = parameters[1].Trim();
            var grade = double.Parse(parameters[2], CultureInfo.InvariantCulture);

            var student = this.school.Students.FirstOrDefault(s => s.FullName == studentName) ??
                          new Student {FullName = studentName};
            student.CoursesGrades.Add(courseName, grade);
        }

        public string ValidateGetSemesterStats(string input)
        {
            this.ValidateStudent(input);
            return this.school.GetSemesterStats(input);
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

        public (Student, Dictionary<string, double>) ValidateAddGradesBulk(string name, string data)
        {
            Student student;
            try
            {
                student = this.ValidateStudent(name);
            }
            catch (NotFoundException e)
            {
                student = new Student {FullName = name};
                this.school.AddStudent(student);
            }

            var coursesGrades = data.Split(";");
            var result = new Dictionary<string, double>();

            foreach (var courseGrade in coursesGrades)
            {
                var parameters = courseGrade.Split(",").Select(s => s.Trim()).ToArray();
                if (parameters.Length != 6)
                {
                    throw new CommandFormatException("The command is not in the specified format. " +
                                                     "Each course data entry must be in the form {semester}, {courseName}, {lecturesHours}, {exercisesHours}, {teacherName}, {grade}");
                }

                try
                {
                    var semester = int.Parse(parameters[0]);
                    var courseName = parameters[1];
                    var lectureHours = int.Parse(parameters[2]);
                    var exerciseHours = int.Parse(parameters[3]);
                    var teacherName = parameters[4];
                    var grade = double.Parse(parameters[5], CultureInfo.InvariantCulture);

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
                    throw new CommandFormatException("The command is not in the specified format.\n" + e.Message);
                }
            }

            return (student, result);
        }


        private Student ValidateStudent(string studentName)
        {
            var student = this.school.Students.FirstOrDefault(s => s.FullName == studentName);
            if (student == null)
            {
                throw new NotFoundException("The student could not be found");
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
using System.Collections.Generic;
using GradeBook.Exceptions;
using GradeBook.Models;
using Xunit;

namespace GradeBook.Tests
{
    public class CommandValidatorTests
    {
        private readonly School school;

        public CommandValidatorTests()
        {
            this.school = new School("TestSchool");
        }

        [Theory]
        [InlineData("add-course", new[] {"add-course"})]
        [InlineData("add-course", new[] {"add-course", ""})]
        [InlineData("add-course", new[] {"add-course", "some, nonesence text, here"})]
        [InlineData("add-course", new[] {"add-course", "1, courseName"})]
        [InlineData("add-course", new[] {"add-course", "1, courseName, 5, notANumber, Dimov"})]
        [InlineData("add-course", new[] {"add-course", "1, courseName, notANumber, 5, Dimov"})]
        [InlineData("add-course", new[] {"add-course", "notANumber, courseName, 5, 5, Dimov"})]
        [InlineData("add-course", new[] {"add-course", "1, , 5, 5, Dimov"})]
        [InlineData("add-course", new[] {"add-course", "1, courseName, 5, 5, "})]
        public void ValidateAddCourse_IncorrectInput_ShouldDisplayError(string command, string[] commandLine)
        {
            Assert.Throws<CommandFormatException>(() => this.school.Validator.ValidateAddCourse(command, commandLine));
        }

        [Fact]
        public void ValidateAddCourse_CorrectInput_ShouldReturn()
        {
            var semesterInput = 1;
            var courseNameInput = "courseName";
            var lectureCountInput = 5;
            var practiceCountInput = 5;
            var teacherNameInput = "Dimov";

            var command = "add-course";
            var commandLine = new[]
            {
                command,
                $"{semesterInput}, {courseNameInput}, {lectureCountInput}, {practiceCountInput}, {teacherNameInput}"
            };

            var expected = (semesterInput, courseNameInput, lectureCountInput, practiceCountInput, teacherNameInput);
            var (semester, courseName, lectureCount, practiceCount, teacherName) =
                this.school.Validator.ValidateAddCourse(command, commandLine);

            var result = expected.semesterInput == semester &&
                         expected.lectureCountInput == lectureCount &&
                         expected.practiceCountInput == practiceCount &&
                         expected.courseNameInput == courseName &&
                         expected.teacherNameInput == teacherName;

            Assert.True(result);
        }

        [Theory]
        [InlineData("add-student", new[] {"add-student"})]
        [InlineData("add-student", new[] {"add-student", ""})]
        [InlineData("add-student", new[] {"add-student", "studentName, Another name"})]
        public void ValidateAddStudent_IncorrectInput_ShouldTrhow(string command, string[] commandLine)
        {
            Assert.Throws<CommandFormatException>(() => this.school.Validator.ValidateAddStudent(command, commandLine));
        }

        [Fact]
        public void ValidateAddStudent_CorrectInput_ShouldReturn()
        {
            const string studentName = "Trendafil Akatsiev";
            const string command = "add-student";
            var actual = this.school.Validator.ValidateAddStudent(command, new[] {command, studentName});
            var expected = studentName;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("add-grade", new[] {"add-grade", "student, course"})]
        [InlineData("add-grade", new[] {"add-grade", "student course 5"})]
        [InlineData("add-grade", new[] {"add-grade"})]
        [InlineData("add-grade", new[] {"add-grade", ""})]
        [InlineData("add-grade", new[] {"add-grade", "student, , 5"})]
        [InlineData("add-grade", new[] {"add-grade", " , , 5"})]
        [InlineData("add-grade", new[] {"add-grade", " ,course , 5"})]
        public void ValidateAddGrade_IncorrectInput_ShouldThrow(string command, string[] commandLine)
        {
            Assert.Throws<CommandFormatException>(() => this.school.Validator.ValidateAddGrade(command, commandLine));
        }

        [Fact]
        public void ValidateAddGrade_CorrectInput_ShouldReturn()
        {
            var command = "add-grade";
            var studentName = "Anakin Skywalker";
            var courseName = "Force Sensitivity";
            var grade = 6;

            var actual =
                this.school.Validator.ValidateAddGrade(command,
                    new[] {command, $"{studentName}, {courseName}, {grade}"});
            Assert.Equal((studentName, courseName, grade), actual);
        }

        [Theory]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", ""}, new[] {""})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"}, new[] {""})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"}, new[] {" , , , , ,"})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"},
            new[] {"1, Guitar mastery, notAnumber, 50, Jimmy Henrix, 6"})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"},
            new[] {"1, Guitar mastery, 50, notAnumber, Jimmy Henrix, 6"})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"},
            new[] {"1, Guitar mastery, 50, 50, ,"})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"},
            new[] {"1, Guitar mastery, 50, 50, Jimmy Hendrix, notANumber"})]
        [InlineData("add-grades-bulk", new[] {"add-grades-bulk", "Bobby Dylan"},
            new[] {"1, Guitar mastery, 50, 50, Jimmy Hendrix, 6; 2, Vocal Mastery, 50, notAnumber, Frank Sinatra, 2"})]
        public void ValidateAddGradesBulk_IncorrectInput_ShouldThrow(string command, string[] commandLine,
            string[] data)
        {
            Assert.Throws<CommandFormatException>(() =>
                this.school.Validator.ValidateAddGradesBulk(command, commandLine, data));
        }

        [Fact]
        public void ValidateAddGradesBulk_CorrectInput_ShouldPass()
        {
            var command = "add-grades-bulk";
            var commandLine = new[] {"add-grades-bulk", "Bobby Dylan"};
            var courseData = new[]
                {"1, Guitar mastery, 50, 50, Jimmy Hendrix, 6", "2, Vocal Mastery, 50, 50, Frank Sinatra, 2"};

            var expected = ("Bobby Dylan", new List<CourseInfo>
            {
                new CourseInfo
                {
                    Semester = 1, LectureCount = 50, Name = "Guitar mastery", PracticeCount = 50,
                    TeacherName = "Jimmy Hendrix", Grade = 6
                },
                new CourseInfo
                {
                    Semester = 2, Name = "Vocal Mastery", LectureCount = 50, PracticeCount = 50,
                    TeacherName = "Frank Sinatra", Grade = 2
                }
            });
            var actual = this.school.Validator.ValidateAddGradesBulk(command, commandLine, courseData);

            var areEqual = expected.Item1 == actual.Student;
            if (areEqual)
            {
                for (var i = 0; i < actual.CourseInfos.Count; i++)
                {
                    if (!actual.CourseInfos[i].Equals(expected.Item2[i]))
                    {
                        areEqual = false;
                        break;
                    }
                }
            }

            Assert.True(areEqual);
        }
    }
}
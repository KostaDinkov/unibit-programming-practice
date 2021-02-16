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
            var semester = 1;
            var courseName = "courseName";
            var lectureCount = 5;
            var practiceCount = 5;
            var teacherName = "Dimov";

            var command = "add-course";
            var commandLine = new[]
                {command, $"{semester}, {courseName}, {lectureCount}, {practiceCount}, {teacherName}"};

            var actual = this.school.Validator.ValidateAddCourse(command, commandLine);

            var expected = new Course
            {
                Semester = semester,
                Name = courseName,
                LectureCount = lectureCount,
                PracticeCount = practiceCount,
                TeacherName = teacherName
            };

            var result = actual.Name == expected.Name &&
                         actual.LectureCount == expected.LectureCount &&
                         actual.PracticeCount == expected.PracticeCount &&
                         actual.Semester == expected.Semester &&
                         actual.TeacherName == expected.TeacherName;

            Assert.True(result);
        }

        [Theory]
        [InlineData("add-student", new[]{"add-student"})]
        [InlineData("add-student", new[] { "add-student","" })]
        [InlineData("add-student", new[] { "add-student","studentName, Another name" })]
        [InlineData("add-student", new[] { "add-student","a very long string that is not a name" })]

        public void ValidateAddStudent_IncorrectInput_ShouldTrhow(string command, string[] commandLine)
        {
            Assert.Throws<CommandFormatException>(() => this.school.Validator.ValidateAddStudent(command, commandLine));
        }
    }
}
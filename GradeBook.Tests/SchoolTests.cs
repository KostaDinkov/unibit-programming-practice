using System.Linq;
using GradeBook.Models;
using Xunit;

namespace GradeBook.Tests
{
    public class SchoolTests
    {
        private readonly School school;

        public SchoolTests()
        {
            this.school = new School("Test School");
            this.school.AddStudent(new Student {FullName = "Angua von Uberwald"});
            this.school.AddCourse(new Course
            {
                Name = "Cooking", InstanceYear = 2021, Semester = 2, LectureCount = 30, PracticeCount = 100,
                TeacherName = "Nanny Ogg"
            });
            this.school.AddCourse(new Course
            {
                Name = "Policemanship", InstanceYear = 2021, Semester = 1, LectureCount = 30, PracticeCount = 100,
                TeacherName = "Lord Vetinari"
            });
            this.school.AddCourse(new Course
            {
                Name = "Thieving", InstanceYear = 2021, Semester = 3, LectureCount = 30, PracticeCount = 100,
                TeacherName = "Urdo van Pew"
            });
            this.school.AddCourse(new Course
            {
                Name = "Fighting", InstanceYear = 2021, Semester = 1, LectureCount = 30, PracticeCount = 100,
                TeacherName = "Sam Vimes"
            });
        }


        [Fact]
        public void AddStudent_ValidInput_ShouldAddToCollection()
        {
            var student = new Student {FullName = "Sam Vimes"};
            this.school.AddStudent(student);
            Assert.Contains(student, this.school.Students);
        }

        [Fact]
        public void AddGrade_ValidInput_ShouldChangeState()
        {
            this.school.AddGrade("Angua von Uberwald", "Policemanship", 6);

            var student = this.school.Students.FirstOrDefault(s => s.FullName == "Angua von Uberwald");
            var actual = student.CoursesGrades["Policemanship"];

            Assert.Equal(6, actual);
        }

        [Fact]
        public void GetGradesString_ValidInput_ShouldReturnString()
        {
            this.school.AddGrade("Angua von Uberwald", "Policemanship", 6);
            this.school.AddGrade("Angua von Uberwald", "Cooking", 3);

            var actual = this.school.GetGradesString("Angua von Uberwald");
            var expected = "Grades for student Angua von Uberwald:\r\n" +
                           "Semester: 1, Course: Policemanship, Teacher: Lord Vetinari, Grade: 6\r\n" +
                           "Semester: 2, Course: Cooking, Teacher: Nanny Ogg, Grade: 3\r\n";

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetSemesterStats_CorrectInput_ShouldReturnString()
        {
            this.school.AddGrade("Angua von Uberwald", "Policemanship", 6);
            this.school.AddGrade("Angua von Uberwald", "Cooking", 3);
            this.school.AddGrade("Angua von Uberwald", "Fighting", 4);
            this.school.AddGrade("Angua von Uberwald", "Thieving", 5);

            var actual = this.school.GetSemesterStats("Angua von Uberwald");

            var expected = "Student: Angua von Uberwald\r\n" +
                           "Total courses: 4\r\n" +
                           "Semester average grades and total study hours:\r\n" +
                           "  1. Semester 1, 260: 5.00\r\n" +
                           "  2. Semester 2, 130: 3.00\r\n" +
                           "  3. Semester 3, 130: 5.00\r\n" +
                           "Total average grade: 4.50\r\n";

            Assert.Equal(expected, actual);
        }
    }
}
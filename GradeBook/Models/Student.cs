using System.Collections.Generic;
using System.Linq;

namespace GradeBook.Models
{
    public class Student


    {
        public Student()
        {
            this.CoursesGrades = new Dictionary<string, double>();
        }

        public string FullName { get; set; }

        public Dictionary<string, double> CoursesGrades { get; }

        public override string ToString()
        {
            return this.FullName;
        }

        public void AddGrade(string courseName, double grade)
        {
            if (this.CoursesGrades.ContainsKey(courseName))
            {
                this.CoursesGrades[courseName] = grade;
            }
            else
            {
                this.CoursesGrades.Add(courseName, grade);
            }
        }

        public double GetAverageGrade()
        {
            return this.CoursesGrades.Average(k => k.Value);
        }
    }
}
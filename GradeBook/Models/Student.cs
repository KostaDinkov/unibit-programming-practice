using System.Collections.Generic;

namespace GradeBook.Models
{
    public class Student

    
    {
        public Student()
        {
            this.CoursesGrades = new Dictionary<string, double>();
        }
        public string FullName { get; set; }
        
        public Dictionary<string, double> CoursesGrades { get; private set; }

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
                this.CoursesGrades.Add(courseName,grade);
            }
        }
    }
}
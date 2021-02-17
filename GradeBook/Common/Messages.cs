using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GradeBook.Common
{
    public static class Messages
    {
        public const string WelcomeMsg = "{0} learning management system.\r\n" +
                                         "For a list of available commands type h:\r\n" +
                                         "Enter Command:";

        public const string StudentAddedMsg = "Student {0} added to school";
        
        public const string CourseAddedMsg = "Course {0} added to school";
        
        public const string ListOfAllCoursesMsg = "List of all courses at {0}";
        
        public const string TotalCoursesMsg = "Total: {0} courses.";

        //get-grades
        public const string GradesForMsg = "Grades for student {0}:";
        public const string GradesLineMsg =
            "Semester: {0}, Course: {1}, Teacher: {2}, Grade: {3}";

        //get-semester-stats
        public const string SemesterStatsMsg = "Student: {0}\r\n" + 
                                               "Total courses: {1}\r\n" +
                                               "Semester average grades and total study hours:";

        public const string SemesterStatsLineMs = "  {0}. Semester {1}, {2}: {3:F2}";
        public const string TotalAverageGradeMsg = "Total average grade: {0:F2}";

        //help
        public const string AvailableCommands = "{0} - available commands:\r\n";

        //command info
        public const string CommandInfoMsg = "{0}\nFormat: {1}\nDescription: {2}\nExample: {3}";

        //Error Messages
        public const string CommandFormatErrorMsg = "Command is not in the specified format.";
        public const string StudentNotFoundMsg = "The student could not be found";

        public const string CommandNotRecognizedMsg =
            "Command Not Recognized. Please enter a valid command or enter h for help";
    }
}

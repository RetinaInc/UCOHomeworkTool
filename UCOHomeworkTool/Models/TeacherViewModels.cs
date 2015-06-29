using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCOHomeworkTool.Models
{
    public class CourseStatistics
    {
        public Course Course{ get; set; }
        public List<AssignmentStatistics> Assignments{ get; set; }
    }
    public class AssignmentStatistics
    {
        public int AssignmentNumber { get; set; }
        public List<ProblemStatistics> Problems { get; set; }
    }
    public class ProblemStatistics
    {
        //each "Try" member variable describes the percentage of students that got this problem correct on that try
        public int ProblemNumber { get; set; }
        public double FirstTry { get; set; }
        public double SecondTry { get; set; }
        public double ThirdTry { get; set; }
        public double FourthTry { get; set; }
        public double FifthTry { get; set; }
    }
}
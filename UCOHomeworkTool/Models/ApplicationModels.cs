using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UCOHomeworkTool.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Assignment> Assignments{ get; set; }
        public virtual List<ApplicationUser> Students { get; set; }
    }
    public class Assignment
    {
        public int Id { get; set; }
        public int AssignmentNumber { get; set; }
        public virtual List<Problem> Problems{ get; set; }
        public virtual Course Course{ get; set; }
    }
    public class Problem
    {
        public int Id { get; set; }
        public int ProblemNumeber{ get; set; }
        public virtual List<Given> Givens{ get; set; }
        public virtual List<Response> Responses { get; set; }
    }
    public class Given
    {
        public int Id { get; set; }
        public string Label{ get; set; }
        public double Value { get; set; }
        public virtual Problem Problem{ get; set; }
    }
    public class Response
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public double Expected{ get; set; }
        public List<double> Actuals { get; set; }
        public virtual Problem Problem{ get; set; }
    }
}
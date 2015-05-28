using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace UCOHomeworkTool.Models
{
    //static RNG members
    public static class Rand
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static double RandomNumber(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Assignment> Templates { get; set; }
        public virtual List<Assignment> Assignments{ get; set; }
        public virtual List<ApplicationUser> Students { get; set; }
    }
    public class Assignment
    {
        public Assignment() { } 
        public Assignment(Assignment toCopy)
        {
            AssignmentNumber = toCopy.AssignmentNumber;
            Course = toCopy.Course;
            Problems = new List<Problem>();
            foreach(var prob in toCopy.Problems)
            {
                var newProb = new Problem(prob);
                Problems.Add(newProb);
            }
        }
        public int Id { get; set; }
        public int AssignmentNumber { get; set; }
        public virtual List<Problem> Problems{ get; set; }
        public virtual Course Course{ get; set; }
        public virtual ApplicationUser Student{ get; set; }
    }
    public class Problem
    {
        public Problem() { }
        public Problem(Problem toCopy)
        {
            ProblemNumber = toCopy.ProblemNumber;
            Givens = new List<Given>();
            GeneratedFrom = toCopy.Id;
            foreach(GivenTemplate given in toCopy.Givens)
            {
                var newGiven = new Given(given);
                newGiven.Problem = this;
                Givens.Add(newGiven);
            }
            Responses = new List<Response>();
            foreach(var resp in toCopy.Responses)
            {
                var newResp = new Response(resp);
                newResp.Problem = this;
                Responses.Add(newResp);
            }
        }
        public int Id { get; set; }
        public int ProblemNumber{ get; set; }
        public int GeneratedFrom { get; set; }
        public virtual List<Given> Givens{ get; set; }
        public virtual List<Response> Responses { get; set; }
    }
    public class ProblemDiagram
    {
        public int Id { get; set; }
        public int ProblemId{ get; set; }
        [NotMapped]
        public Image Diagram { get; set; }
        public byte[] ImageContent 
        { 
            get
            {
                MemoryStream ms = new MemoryStream();
                Diagram.Save(ms, Diagram.RawFormat);
                return ms.ToArray();
            }
            set
            {
                MemoryStream ms = new MemoryStream(value);
                Diagram = Image.FromStream(ms);
            }
        }
    }
    public class Given
    {
        public Given() { }
        public Given(GivenTemplate toCopy)
        {
            Label = toCopy.Label;
            Random rand = new Random();
            Value = Rand.RandomNumber(toCopy.minRange, toCopy.maxRange);
        }
        public int Id { get; set; }
        public string Label{ get; set; }
        public double Value { get; set; }
        public virtual Problem Problem{ get; set; }
    }
    public class GivenTemplate : Given
    {
        public double minRange { get; set; }
        public double maxRange { get; set; }
    }
    public class Response
    {
        public Response() { }
        public Response(Response toCopy)
        {
            Label = toCopy.Label;
        }
        public int Id { get; set; }
        public string Label { get; set; }
        public double Expected{ get; set; }
        public List<double> Actuals { get; set; }
        public virtual Problem Problem{ get; set; }
    }
}
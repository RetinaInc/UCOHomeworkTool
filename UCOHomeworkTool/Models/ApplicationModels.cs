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
        public static int RandomNumber(int min, int max)
        {
            return random.Next(min,max) ;
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
            TrysRemaining = 3;
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
                //setting expected to whatever value is stored in the template
                newResp.Expected = resp.Expected;
                //overriding the default only if an applicable equation exists to calculate the expected value
                newResp.setExpected(this.Givens);
                newResp.Problem = this;
                Responses.Add(newResp);
            }
        }
        public bool AllResponsesCorrect()
        {
            bool allCorrect = true;
            foreach(var response in this.Responses)
            {
                switch(this.TrysRemaining)
                {
                    case 3:
                        allCorrect = false;
                        break;
                    case 2:
                        allCorrect = response.Expected.Equals(response.FirstAttempt);
                        break;
                    case 1:
                        allCorrect = response.Expected.Equals(response.SecondAttempt);
                        break;
                    case 0:
                        allCorrect = response.Expected.Equals(response.ThirdAttempt);
                        break;
                }
            }
            return allCorrect;
        }
        public int Id { get; set; }
        public int ProblemNumber{ get; set; }
        public int GeneratedFrom { get; set; }
        public int TrysRemaining { get; set; }
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
        public int Value { get; set; }
        public virtual Problem Problem{ get; set; }
    }
    public class GivenTemplate : Given
    {
        public int minRange { get; set; }
        public int maxRange { get; set; }
    }
    public class Response
    {
        public Response() { }
        public Response(Response toCopy)
        {
            Label = toCopy.Label;
        }
        public void setExpected(List<Given> givens)
        {
            //find and assign appropriate givens for this problem
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given V1given = givens.Find(g => g.Label == "V1") ?? new Given { Value = 0 };
            Given V2given = givens.Find(g => g.Label == "V2") ?? new Given { Value = 0 };
            double R1 = (double) R1given.Value;
            double R2 = (double) R2given.Value;
            double R3 = (double) R3given.Value;
            double V1 = (double) V1given.Value;
            double V2 = (double) V2given.Value;
            //make sure none of the values are 0
            if (R1 * R2 * R3 * V1 * V2 == 0)
                return; 
            //calculate V0
            double V0 = ((V1 / R1) + (V2 / R3)) * (1 / ((1 / R1) + (1 / R2) + (1 / R3)));
            //based on what response we are trying to find, use the correct equation
            if(this.Label == "i1")
            {
                Expected = Math.Round(((V0 - V1) / R1),2, MidpointRounding.AwayFromZero);
            }
            else if(this.Label == "i2")
            {
                Expected = Math.Round((V0 / R2),2, MidpointRounding.AwayFromZero);
            }
            else if(this.Label == "i3")
            {
                Expected = Math.Round(((V0 - V2) / R3),2, MidpointRounding.AwayFromZero);
            }
        }
        public int Id { get; set; }
        public string Label { get; set; }
        public double Expected{ get; set; }
        public double FirstAttempt { get; set; }
        public double SecondAttempt{ get; set; }
        public double ThirdAttempt{ get; set; }

        public virtual Problem Problem{ get; set; }
    }
}
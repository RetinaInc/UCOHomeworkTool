using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Calculations
{
    public class Calculations
    {
        public static Problem.CalculateResponseDelegate GetCalculation(string calcString, string courseName)
        {
            Type type;
            switch (courseName.ToLower())
            {
                case "physics":
                    type = typeof(Physics);
                    break;
                case "electrical science":
                    type = typeof(ElectricalScience);
                    break;
                case "signals":
                    type = typeof(Signals);
                    break;
                case "physics for engineers and scientist 1":
                    type = typeof(PSE1);
                    break;
                default:
                    type = null;
                    break;
            }
            if (type == null)
            {
                return null;
            }
            return (Problem.CalculateResponseDelegate)Delegate.CreateDelegate(typeof(Problem.CalculateResponseDelegate), type, calcString);
        }
        public static void SetExpectedResponse(Response response, string label, double value)
        {
            if (response.Label == label)
            {
                response.Expected = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static double GetGivenValue(List<Given> givens, string label)
        {

            Given given = givens.Find(g => g.Label == label) ?? new Given { Value = 0 };
            return (double) given.Value;

        }
    }
}
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
            switch(courseName.ToLower())
            {
                case "electrical science":
                    type = typeof(ElectricalScience);
                    break;
                case "signals":
                    type = typeof(Signals);
                    break;
                default:
                    type = null;
                    break;
            }
            if (type == null)
            {
                return null;
            }
            return (Problem.CalculateResponseDelegate)Delegate.CreateDelegate(typeof (Problem.CalculateResponseDelegate), type, calcString);
        }
        public static void SetExpectedResponse(Response response, string label, double value)
        {
            if (response.Label == label)
            {
                response.Expected = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}
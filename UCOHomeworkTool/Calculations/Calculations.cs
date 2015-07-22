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
            //old way of getting delegates
            //switch(calcString.ToLower())
            //{
            //    case "electricalscience.a1p1":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a1p1);
            //    case "electricalscience.a1p2":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a1p2);
            //    case "electricalscience.a1p3":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a1p3);
            //    case "electricalscience.a1p4":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a1p4);
            //    case "electricalscience.a2p1":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a2p1);
            //    case "electricalscience.a2p2":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a2p2);
            //    case "electricalscience.a2p3":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a2p3);
            //    case "electricalscience.a3p1":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a3p1);
            //    case "electricalscience.a3p2":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a3p2);
            //    case "electricalscience.a3p3":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a3p3);
            //    case "electricalscience.a4p1":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a4p1);
            //    case "electricalscience.a4p2":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a4p2);
            //    case "electricalscience.a4p3":
            //        return new Problem.CalculateResponseDelegate(ElectricalScience.a4p3);
            //    default:
            //        return null;
            //}
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
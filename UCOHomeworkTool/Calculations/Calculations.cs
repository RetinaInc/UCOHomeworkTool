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

        public static void a5p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given V1given = givens.Find(g => g.Label == "V1") ?? new Given { Value = 0 };
            Given V2given = givens.Find(g => g.Label == "V2") ?? new Given { Value = 0 };
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double V1 = (double)V1given.Value;
            double V2 = (double)V2given.Value;
            //make sure none of the values are 0
            if (R1 * R2 * R3 * V1 * V2 == 0)
                return;
            //calculate V0
            double V0 = ((V1 / R1) + (V2 / R3)) * (1 / ((1 / R1) + (1 / R2) + (1 / R3)));
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "i1")
            {
                toCalculate.Expected = Math.Round(((V0 - V1) / R1), 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "i2")
            {
                toCalculate.Expected = Math.Round((V0 / R2), 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "i3")
            {
                toCalculate.Expected = Math.Round(((V0 - V2) / R3), 2, MidpointRounding.AwayFromZero);
            }
        }

    }
}
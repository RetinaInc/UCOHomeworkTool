using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Calculations
{
    public class Signals
    {
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
            Calculations.SetExpectedResponse(toCalculate, "i1", ((V0 - V1) / R1));
            Calculations.SetExpectedResponse(toCalculate, "i2", (V0 / R2));
            Calculations.SetExpectedResponse(toCalculate, "i3", ((V0 - V2) / R3));
        }
    }
}
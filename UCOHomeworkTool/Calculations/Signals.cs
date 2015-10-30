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
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double V1 = Calculations.GetGivenValue(givens, "V1");
            double V2 = Calculations.GetGivenValue(givens, "V2");

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
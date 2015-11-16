using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Calculations
{
    public class PSE1
    {
        public static void a1p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double N = Calculations.GetGivenValue(givens, "N");
            double x = Calculations.GetGivenValue(givens, "x");
            double B = Calculations.GetGivenValue(givens, "B");
            double V = Calculations.GetGivenValue(givens, "V");
            //make sure none of the values are 0
            if (N * x * B * V == 0)
                return;
            //calculate intermediate values 
            double A = (x * x) / 10000;
            double w = (3.1414*V)/(2*N*B*A);


            //based on what response we are trying to find, use the correct equation
            Calculations.SetExpectedResponse(toCalculate, "w", w);

        }
    }
}
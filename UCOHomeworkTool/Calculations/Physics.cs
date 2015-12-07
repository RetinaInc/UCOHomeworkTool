using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Calculations
{
    public class Physics
    {
        public static void a1p1(List<Given> givens, Response response)
        {
            double A = Calculations.GetGivenValue(givens, "A");
            double M = Calculations.GetGivenValue(givens, "M");

            double F = A * M;

            Calculations.SetExpectedResponse(response, "F", F);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Calculations
{
    public class ElectricalScience
    {
        public static void a1p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs1");
            double Vs2 = Calculations.GetGivenValue(givens, "Vs2");
            double Vs3 = Calculations.GetGivenValue(givens, "Vs3");

            //make sure none of the values are 0
            if (Vs1 * Vs2 * Vs3 == 0)
                return;
            //calculate intermediate values 
            double v1 = Vs1 - Vs2 - Vs3;
            double v2 = -Vs2 - Vs3;
            double v3 = Vs3;

            //based on what response we are trying to find, use the correct equation
            Calculations.SetExpectedResponse(toCalculate, "v1", v1);
            Calculations.SetExpectedResponse(toCalculate, "v2", v2);
            Calculations.SetExpectedResponse(toCalculate, "v3", v3);
        }
        public static void a1p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs1");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double a = Calculations.GetGivenValue(givens, "a");

            //make sure none of the values are 0
            if (Vs1 * R1 * R2 * R3 * a == 0)
                return;

            //calculate intermediate values 
            var I = Vs1 / (R1 + a * R2 + R2 + R3);
            var Vx = R2 * I;
            var P = I * a * Vx;

            //set correct answer for responses
            Calculations.SetExpectedResponse(toCalculate, "Vx", Vx);
            Calculations.SetExpectedResponse(toCalculate, "P", P);
        }
        public static void a1p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs = Calculations.GetGivenValue(givens, "Vs");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            //make sure none of the values are 0
            if (Vs * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var R13 = R1 * R3 / (R1 + R3);
            var R24 = R2 * R4 / (R2 + R4);
            var RT = R13 + R24;
            var Is = Vs / RT;
            var V1 = R13 * Is;
            var V2 = R24 * Is;
            var I1 = V1 / R1;
            var I2 = V2 / R2;
            var I0 = I1 - I2;
            var V0 = V2;

            //based on what response we are trying to find, use the correct equation
            Calculations.SetExpectedResponse(toCalculate, "V0", V0);
            Calculations.SetExpectedResponse(toCalculate, "I0", I0);

        }
        public static void a1p4(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vb = Calculations.GetGivenValue(givens, "Vb");
            double Ib = Calculations.GetGivenValue(givens, "Ib");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");

            //make sure none of the values are 0
            if (Vb * Ib * R1 * R2 == 0)
                return;

            //calculate intermediate values 
            var Ir2 = Vb / R2;
            var Ir1 = Ib + Ir2;
            var Vs = R1 * Ir1 + Vb;

            //Set calculated correct answer for responses
            Calculations.SetExpectedResponse(toCalculate, "Vs", Vs);
        }
        public static void a2p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs = Calculations.GetGivenValue(givens, "Vs");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double a = Calculations.GetGivenValue(givens, "a");

            //make sure none of the values are 0
            if (Vs * R1 * R2 * R3 * R4 * a == 0)
                return;

            //calculate intermediate values 
            var C1 = R2 * R4 + a * R3 * R4;
            var C2 = R2 * R3 + R1 * R3 + R1 * R2;
            var C3 = R2 * R4 + R2 * R3;
            var Vo = C1 * Vs * R2 * R3 / (C2 * C3 - R1 * R2 * C1);
            var V1 = Vo * C3 / C1;
            var Ix = V1 / R2;

            Calculations.SetExpectedResponse(toCalculate, "Vo", Vo);
            Calculations.SetExpectedResponse(toCalculate, "Ix", Ix);
        }
        public static void a2p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs1");
            double Vs2 = Calculations.GetGivenValue(givens, "Vs2");
            double Is = Calculations.GetGivenValue(givens, "Is");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R3");
            
            //make sure none of the values are 0
            if (Vs1 * Vs2 * Is * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var C1 = R1 + R3 + R4;
            var C2 = Vs2 + R4 * Is;
            var C3 = Vs1 - R4 * Is;
            var C4 = R3 + R4;
            var C5 = R2 + R3 + R4;
            var I2 = (C2 * C1 + C3 * C4) / (C5 * C1 - C4 * C4);
            var I1 = (C3 + I2 * C4) / C1;
            var I3 = I2 - Is;
            var i0 = I1 - I2;
            var i1 = -I3;

            Calculations.SetExpectedResponse(toCalculate, "i0", i0);
            Calculations.SetExpectedResponse(toCalculate, "i1", i1);
        }
        public static void a2p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs");
            double Is = Calculations.GetGivenValue(givens, "Is");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double R5 = Calculations.GetGivenValue(givens, "R5");
            
            //make sure none of the values are 0
            if (Vs1 * R5 * Is * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var C1 = R2 / (R1 + R2);
            var I2 = -C1 * Is;
            var I1 = (6 * I2 + Vs1 + (6 - R4) * Is) / R4;
            var V2 = R4 * (I1 + Is);
            var V1 = V2 + R3 * Is;
            var V3 = Vs1;
            var V4 = R1 * I2;

            Calculations.SetExpectedResponse(toCalculate, "V1", V1);
            Calculations.SetExpectedResponse(toCalculate, "V2", V2);
            Calculations.SetExpectedResponse(toCalculate, "V3", V3);
            Calculations.SetExpectedResponse(toCalculate, "V4", V4);
        }
        public static void a3p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs1");
            double Vs2 = Calculations.GetGivenValue(givens, "Vs2");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double Is3 = Calculations.GetGivenValue(givens, "Is3");

            //make sure none of the values are 0
            if (Vs1 * Is3 * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var V = (Vs1 / R1) / (1 / R1 + 1 / (R2 + R3) + 1 / R4);
            var is1 = V / (R2 + R3);
            var V1 = (Vs2 / R4) / (1 / R1 + 1 / R3 + 1 / R4);
            var is2 = -V1 / (R2 + R3);
            var is3 = Is3 * (R2 / (R2 + R3 + (R1 * R4 / (R1 + R4))));
            var i = is1 + is2 + is3;

            Calculations.SetExpectedResponse(toCalculate, "i due to source Vs1", is1);
            Calculations.SetExpectedResponse(toCalculate, "i due to source Vs2", is2);
            Calculations.SetExpectedResponse(toCalculate, "i due to source Is3", is3);
        }
        public static void a3p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs1");
            double Vs2 = Calculations.GetGivenValue(givens, "Vs2");
            double Is = Calculations.GetGivenValue(givens, "Is");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");

            //make sure none of the values are 0
            if (Vs1 * Vs2 * Is * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var C1 = R1 * R2 / (R1 + R2);
            var V1 = C1 * Vs1 / R1;
            var V2 = R4 * Is + Vs2;
            var I1 = (V1 - V2) / (V1 + R3 + R4);
            var Vx = R3 * I1;

            Calculations.SetExpectedResponse(toCalculate, "Vx", Vx);
        }
        public static void a3p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs = Calculations.GetGivenValue(givens, "Vs");
            double Is = Calculations.GetGivenValue(givens, "Is");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double R5 = Calculations.GetGivenValue(givens, "R5");
            
            //make sure none of the values are 0
            if (Vs * R5 * Is * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var Rt = (R1 + R2 + R3 + R4);
            var C = (R1 + R2 + R4) * R3 / Rt;
            var Rth_ab = R5 + C;
            var I1 = (Vs - R4 * Is) / Rt;
            var Vth_ab = R3 * I1;
            var Rth_bc = (R1 + R2 + R3) * R4 / Rt;
            var Vth_bc = R4 * (I1 + Is);

            Calculations.SetExpectedResponse(toCalculate, "Vth_ab", Vth_ab);
            Calculations.SetExpectedResponse(toCalculate, "Rth_ab", Rth_ab);
            Calculations.SetExpectedResponse(toCalculate, "Vth_bc", Vth_bc);
            Calculations.SetExpectedResponse(toCalculate, "Rth_bc", Rth_bc);

        }
        public static void a4p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs = Calculations.GetGivenValue(givens, "Vs");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double R5 = Calculations.GetGivenValue(givens, "R5");

            //make sure none of the values are 0
            if (Vs * R5 * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var V = (Vs / R1) / (1 / R1 + 1 / R2 + 1 / R3);
            var V0 = -V * (R4 / R3);
            var io = V0 / R5 - V / (R3 + R4);

            Calculations.SetExpectedResponse(toCalculate, "io (in microAmp)", io);
        }
        public static void a4p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double Vs1 = Calculations.GetGivenValue(givens, "Vs1");
            double Vs2 = Calculations.GetGivenValue(givens, "Vs2");
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double R5 = Calculations.GetGivenValue(givens, "R5");

            //make sure none of the values are 0
            if (Vs1 * R5 * Vs2 * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var Vo1 = -Vs1 * R2 / R1;
            var Vo = -(R5 * Vs2 / R4 + R5 * Vo1 / R3);

            Calculations.SetExpectedResponse(toCalculate, "Vo", Vo);
        }
        public static void a4p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            double R1 = Calculations.GetGivenValue(givens, "R1");
            double R2 = Calculations.GetGivenValue(givens, "R2");
            double R3 = Calculations.GetGivenValue(givens, "R3");
            double R4 = Calculations.GetGivenValue(givens, "R4");
            double R5 = Calculations.GetGivenValue(givens, "R5");

            //make sure none of the values are 0
            if (R5 * R1 * R2 * R3 * R4 == 0)
                return;

            //calculate intermediate values 
            var C1 = 1 + R5 / R4;
            var C2 = R2 / R1;
            var C3 = R3 / R1;
            //calc Vo/Vi and store in G
            var G = -(C1 * C2) / (1 + C1 * C3);

            Calculations.SetExpectedResponse(toCalculate, "Vo/Vi", G);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Calculations
{
    public class Calculations
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
        public static void a1p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vs1given = givens.Find(g => g.Label == "Vs1") ?? new Given { Value = 0 };
            Given Vs2given = givens.Find(g => g.Label == "Vs2") ?? new Given { Value = 0 };
            Given Vs3given = givens.Find(g => g.Label == "Vs3") ?? new Given { Value = 0 };
            double Vs1 = (double)Vs1given.Value;
            double Vs2 = (double)Vs2given.Value;
            double Vs3 = (double)Vs3given.Value;
            //make sure none of the values are 0
            if (Vs1 * Vs2 * Vs3 == 0)
                return;
            //calculate intermediate values 

            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "v1")
            {
                toCalculate.Expected = Math.Round(Vs1-Vs2-Vs3, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "v2")
            {
                toCalculate.Expected = Math.Round(-Vs2 - Vs3, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "v3")
            {
                toCalculate.Expected = Math.Round(Vs3, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a1p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vs1given = givens.Find(g => g.Label == "Vs1") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given agiven = givens.Find(g => g.Label == "a") ?? new Given { Value = 0 };
            double Vs1 = (double)Vs1given.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double a = (double)agiven.Value;
            //make sure none of the values are 0
            if (Vs1 * R1* R2 * R3 * a == 0)
                return;
            //calculate intermediate values 
            var I = Vs1 / (R1 + a * R2 + R2 + R3);
            var Vx = R2 * I;
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "Vx")
            {
                toCalculate.Expected = Math.Round(Vx, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "P")
            {
                toCalculate.Expected = Math.Round(I * a * Vx, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a1p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vsgiven = givens.Find(g => g.Label == "Vs") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            double Vs = (double)Vsgiven.Value;
            double R1= (double)R1given.Value;
            double R2= (double)R2given.Value;
            double R3= (double)R3given.Value;
            double R4= (double)R4given.Value;
            //make sure none of the values are 0
            if (Vs * R1* R2 * R3 * R4 == 0)
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
            if (toCalculate.Label == "V0")
            {
                toCalculate.Expected = Math.Round(V0, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "I0")
            {
                toCalculate.Expected = Math.Round(I0, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a1p4(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vbgiven = givens.Find(g => g.Label == "Vb") ?? new Given { Value = 0 };
            Given Ibgiven = givens.Find(g => g.Label == "Ib") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            double Vb = (double)Vbgiven.Value;
            double Ib = (double)Ibgiven.Value;
            double R1= (double)Ibgiven.Value;
            double R2= (double)Ibgiven.Value;
            //make sure none of the values are 0
            if (Vb * Ib * R1 * R2 == 0)
                return;
            //calculate intermediate values 
            var Ir2 = Vb / R2;
            var Ir1 = Ib + Ir2;
            var Vs = R1 * Ir1 + Vb;
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "Vs")
            {
                toCalculate.Expected = Math.Round(Vs, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a2p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vsgiven = givens.Find(g => g.Label == "Vs") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given agiven = givens.Find(g => g.Label == "a") ?? new Given { Value = 0 };
            double Vs = (double)Vsgiven.Value;
            double R1= (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double a = (double)agiven.Value;
            //make sure none of the values are 0
            if (Vs * R1 * R2 * R3 * R4 * a == 0)
                return;
            //calculate intermediate values 
            var C1 = R2 * R4 + a * R3 * R4;
            var C2 = R2 * R3 + R1 * R3 + R1 * R2;
            var C3 = R2 * R4 + R2 * R3;
            var V0 = C1 * Vs * R2 * R3 / (C2 * C3 - R1 * R2 * C1);
            var V1 = V0 * C3 / C1;
            var Ix = V1 / R2;
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "V0")
            {
                toCalculate.Expected = Math.Round(V0, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "Ix")
            {
                toCalculate.Expected = Math.Round(Ix, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a2p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vs1given = givens.Find(g => g.Label == "Vs1") ?? new Given { Value = 0 };
            Given Vs2given = givens.Find(g => g.Label == "Vs2") ?? new Given { Value = 0 };
            Given Isgiven = givens.Find(g => g.Label == "Is") ?? new Given { Value = 0 };
            Given R1given= givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given= givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given= givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given= givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            double Vs1 = (double)Vs1given.Value;
            double Vs2= (double)Vs2given.Value;
            double Is= (double)Isgiven.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            //make sure none of the values are 0
            if (Vs1 *Vs2 * Is * R1 * R2 * R3 * R4 == 0)
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
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "i0")
            {
                toCalculate.Expected = Math.Round(i0, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "i1")
            {
                toCalculate.Expected = Math.Round(i1, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a2p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vsgiven = givens.Find(g => g.Label == "Vs") ?? new Given { Value = 0 };
            Given Isgiven = givens.Find(g => g.Label == "Is") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given R5given = givens.Find(g => g.Label == "R5") ?? new Given { Value = 0 };
            double Vs1 = (double)Vsgiven.Value;
            double Is = (double)Isgiven.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double R5 = (double)R5given.Value;
            //make sure none of the values are 0
            if (Vs1 * R5* Is * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var C1 = R2 / (R1 + R2);
            var I2 = -C1 * Is;
            var I1 = (6 * I2 + Vs1 + (6 - R4) * Is) / R4;
            var V2 = R4 * (I1 + Is);
            var V1 = V2 + R3 * Is;
            var V3 = Vs1;
            var V4 = R1 * I2;
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "V1")
            {
                toCalculate.Expected = Math.Round(V1, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "V2")
            {
                toCalculate.Expected = Math.Round(V2, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "V3")
            {
                toCalculate.Expected = Math.Round(V3, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "V4")
            {
                toCalculate.Expected = Math.Round(V4, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a3p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vs1given = givens.Find(g => g.Label == "Vs1") ?? new Given { Value = 0 };
            Given Vs2given = givens.Find(g => g.Label == "Vs2") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given Is3given = givens.Find(g => g.Label == "Is3") ?? new Given { Value = 0 };
            double Vs1 = (double)Vs1given.Value;
            double Vs2 = (double)Vs2given.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double Is3 = (double)Is3given.Value;
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
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "i due to source Vs1")
            {
                toCalculate.Expected = Math.Round(is1, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "i due to source Vs2")
            {
                toCalculate.Expected = Math.Round(is2, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "i due to source Is3")
            {
                toCalculate.Expected = Math.Round(is3, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "i total")
            {
                toCalculate.Expected = Math.Round(i, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a3p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vs1given = givens.Find(g => g.Label == "Vs1") ?? new Given { Value = 0 };
            Given Vs2given = givens.Find(g => g.Label == "Vs2") ?? new Given { Value = 0 };
            Given Isgiven = givens.Find(g => g.Label == "Is") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            double Vs1 = (double)Vs1given.Value;
            double Vs2 = (double)Vs2given.Value;
            double Is = (double)Isgiven.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            //make sure none of the values are 0
            if (Vs1 * Vs2* Is * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var C1 = R1 * R2 / (R1 + R2);
            var V1 = C1 * Vs1 / R1;
            var V2 = R4 * Is + Vs2;
            var I1 = (V1 - V2) / (V1 + R3 + R4);
            var Vx = R3 * I1;
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "Vx")
            {
                toCalculate.Expected = Math.Round(Vx, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a3p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vsgiven = givens.Find(g => g.Label == "Vs") ?? new Given { Value = 0 };
            Given Isgiven = givens.Find(g => g.Label == "Is") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given R5given = givens.Find(g => g.Label == "R5") ?? new Given { Value = 0 };
            double Vs = (double)Vsgiven.Value;
            double Is = (double)Isgiven.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double R5 = (double)R5given.Value;
            //make sure none of the values are 0
            if (Vs * R5* Is * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var Rt = (R1 + R2 + R3 + R4);
            var C = (R1 + R2 + R4) * R3 / Rt;
            var Rth_ab = R5 + C;
            var I1 = (Vs - R4 * Is) / Rt;
            var Vth_ab = R3 * I1;
            var Rth_bc = (R1 + R2 + R3) * R4 / Rt;
            var Vth_bc = R4 * (I1 + Is);
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "Vth_ab")
            {
                toCalculate.Expected = Math.Round(Vth_ab, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "Rth_ab")
            {
                toCalculate.Expected = Math.Round(Rth_ab, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "Vth_bc")
            {
                toCalculate.Expected = Math.Round(Vth_bc, 2, MidpointRounding.AwayFromZero);
            }
            else if (toCalculate.Label == "Rth_bc")
            {
                toCalculate.Expected = Math.Round(Rth_bc, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a4p1(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vsgiven = givens.Find(g => g.Label == "Vs") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given R5given = givens.Find(g => g.Label == "R5") ?? new Given { Value = 0 };
            double Vs = (double)Vsgiven.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double R5 = (double)R5given.Value;
            //make sure none of the values are 0
            if (Vs * R5 * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var V = (Vs / R1) / (1 / R1 + 1 / R2 + 1 / R3);
            var V0 = -V * (R4 / R3);
            var io = V0 / R5 - V / (R3 + R4);
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "io (in microAmp)")
            {
                toCalculate.Expected = Math.Round(io, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a4p2(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given Vs1given = givens.Find(g => g.Label == "Vs1") ?? new Given { Value = 0 };
            Given Vs2given = givens.Find(g => g.Label == "Vs2") ?? new Given { Value = 0 };
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given R5given = givens.Find(g => g.Label == "R5") ?? new Given { Value = 0 };
            double Vs1 = (double)Vs1given.Value;
            double Vs2= (double)Vs2given.Value;
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double R5 = (double)R5given.Value;
            //make sure none of the values are 0
            if (Vs1 * R5 * Vs2 * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var Vo1 = -Vs1 * R2 / R1;
            var Vo = -(R5 * Vs2 / R4 + R5 * Vo1 / R3);
            
            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "Vo")
            {
                toCalculate.Expected = Math.Round(Vo, 2, MidpointRounding.AwayFromZero);
            }
        }
        public static void a4p3(List<Given> givens, Response toCalculate)
        {
            //find and assign appropriate givens for this problem
            Given R1given = givens.Find(g => g.Label == "R1") ?? new Given { Value = 0 };
            Given R2given = givens.Find(g => g.Label == "R2") ?? new Given { Value = 0 };
            Given R3given = givens.Find(g => g.Label == "R3") ?? new Given { Value = 0 };
            Given R4given = givens.Find(g => g.Label == "R4") ?? new Given { Value = 0 };
            Given R5given = givens.Find(g => g.Label == "R5") ?? new Given { Value = 0 };
            double R1 = (double)R1given.Value;
            double R2 = (double)R2given.Value;
            double R3 = (double)R3given.Value;
            double R4 = (double)R4given.Value;
            double R5 = (double)R5given.Value;
            //make sure none of the values are 0
            if (R5 * R1 * R2 * R3 * R4 == 0)
                return;
            //calculate intermediate values 
            var C1 = 1 + R5 / R4;
            var C2 = R2 / R1;
            var C3 = R3 / R1;
            var G = -(C1 * C2) / (1 + C1 * C3);

            //based on what response we are trying to find, use the correct equation
            if (toCalculate.Label == "Vo/Vi")
            {
                toCalculate.Expected = Math.Round(G, 2, MidpointRounding.AwayFromZero);
            }
        }
    }
}
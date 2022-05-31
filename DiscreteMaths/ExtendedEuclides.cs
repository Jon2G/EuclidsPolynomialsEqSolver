namespace DiscreteMaths
{
    public class ExtendedEuclides
    {
        public readonly PolynomialEq Hx;
        public readonly PolynomialEq Gx;
        /// <summary>
        /// Máximo común divisor
        /// </summary>
        public PolynomialEq Dx;
        public PolynomialEq Sx;
        public PolynomialEq Tx;
        public readonly List<string> Steps;
        public ExtendedEuclides(PolynomialEq gx, PolynomialEq hx)
        {
            this.Steps = new List<string>();
            this.Gx = gx; //debe ser el de mayor grado
            this.Hx = hx;
            Console.WriteLine(Gx);
            Console.WriteLine(Hx);
        }

        private void AddSteps(params string[] steps)
        {
            Steps.AddRange(steps);
        }
        public void Solve()
        {
            //por formula
            PolynomialEq gx = this.Gx.Clone();
            PolynomialEq hx = this.Hx.Clone();
            PolynomialEq s2 = new PolynomialEq("s2", XTerm.One);
            PolynomialEq s1 = new PolynomialEq("s1", XTerm.Zero);
            PolynomialEq t2 = new PolynomialEq("t2", XTerm.Zero);
            PolynomialEq t1 = new PolynomialEq("t1", XTerm.One);
            AddSteps(s2.ToString(), s1.ToString(), t2.ToString(), t1.ToString());
            while (hx.IsNotZero)
            {
                AddSteps($"Teniendo que ({hx}) es diferente de cero");
                //9
                AddSteps($"Dividimos {gx} entre {hx}");
                AddSteps($"<div>\n<p style=\"margin: 0;\">{gx.ToString()}</p>\n<div style=\"background: black;height: 1px;width: 200px;margin: 0;\"></div>\n<p>{hx.ToString()}</p>\n</div>");
                PolynomialDivisionResult qxDiv = gx / hx;
                PolynomialEq qx = qxDiv.Result.Mod().SetLetter('q');
                PolynomialEq rx = qxDiv.Remainder.Mod().SetLetter('r');
                AddSteps("Resultado de la división:", qx.ToString(), "Resusido de la división:", rx.ToString());
                //10
                AddSteps("Hacemos a sx=(s2 - qx * s1)", $"sx=({s2}-({qx}*{s1}))");
                PolynomialEq sx = (s2 - (qx * s1)).Mod().SetLetter('s');
                AddSteps(sx.ToString());
                PolynomialEq tx = (t2 - (qx * t1)).Mod().SetLetter('t');
                AddSteps("Hacemos a tx=(s2 - qx * s1)", $"tx=({t2}-({qx}*{t1}))");
                //11
                AddSteps("Hacemos a gx = hx y hx = rx");
                gx = hx.Clone().SetLetter('g');
                hx = rx.Clone().SetLetter('h');
                AddSteps(gx.ToString(), hx.ToString());
                //12
                AddSteps("Hacemos a s2 = s1 , s1=sx, t2=t1, t1 = tx");
                s2 = s1.Clone().SetLetter("s2");
                s1 = sx.Clone().SetLetter("s1");
                t2 = t1.Clone().SetLetter("t2");
                t1 = tx.Clone().SetLetter("t1");
                AddSteps(s2.ToString(), s1.ToString(), t2.ToString(), t1.ToString());
            }
            AddSteps($"{hx} es cero por lo tanto hemos terminado ...");
            //14
            Dx = gx;
            Sx = s2;
            Tx = t2;
        }
    }
}

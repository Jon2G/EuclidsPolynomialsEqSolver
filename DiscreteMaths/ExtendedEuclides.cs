using Microsoft.AspNetCore.Components;

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
        public readonly List<Step> Steps;
        public ExtendedEuclides(PolynomialEq gx, PolynomialEq hx)
        {
            this.Steps = new List<Step>();
            this.Gx = gx; //debe ser el de mayor grado
            this.Hx = hx;
            Console.WriteLine(Gx);
            Console.WriteLine(Hx);
        }

        private void AddStepsBlock(int number) => Steps.Add(new Step() { Number = number });
        private void AddBoldSteps(params string[] steps)
        {
            Steps.Last()
                .AddRange(steps.Select(x => (MarkupString)$"<b>{x}</b>"));
        }

        private void AddSteps(params MarkupString[] steps)
        {
            Steps.Last()
                .AddRange(steps);
        }
        private void AddSteps(params string[] steps)
        {
            AddSteps(steps.Select(x => (MarkupString)x).ToArray());
        }
        public void Solve()
        {
            int steps = 0;
            AddStepsBlock(steps);
            //por formula
            PolynomialEq gx = this.Gx.Clone();
            PolynomialEq hx = this.Hx.Clone();
            PolynomialEq s2 = new PolynomialEq("S_2", XTerm.One);
            PolynomialEq s1 = new PolynomialEq("S_1", XTerm.Zero);
            PolynomialEq t2 = new PolynomialEq("T_", XTerm.Zero);
            PolynomialEq t1 = new PolynomialEq("T_1", XTerm.One);
            AddSteps(s2.ToLatexString(), s1.ToLatexString(), t2.ToLatexString(), t1.ToLatexString());
            while (hx.IsNotZero)
            {
                AddStepsBlock(++steps);
                AddBoldSteps($"Teniendo que $${hx}$$ es diferente de cero");
                //9
                AddBoldSteps($"Dividimos $$G(x)$$ entre $$H(x)$$");
                AddSteps(@$"$$\frac{{{gx.ToString(false)}}}{{{hx.ToString(false)}}}$$");
                PolynomialDivisionResult qxDiv = gx / hx;
                PolynomialEq qx = qxDiv.Result.Mod().SetLetter('q');
                PolynomialEq rx = qxDiv.Remainder.Mod().SetLetter('r');
                AddBoldSteps("Resultado de la división:");
                AddSteps(qx.ToLatexString());
                AddBoldSteps("Resusido de la división:");
                AddSteps(rx.ToLatexString());
                //10
                AddBoldSteps("Hacemos a $$S(x)=(S_2 - Q(x) * S_1)$$");
                AddSteps($"$$S(x)=({s2.ToString(false)}-({qx.ToString(false)}*{s1.ToString(false)}))$$");
                PolynomialEq sx = (s2 - (qx * s1)).Mod().SetLetter('s');
                AddSteps(sx.ToLatexString());
                PolynomialEq tx = (t2 - (qx * t1)).Mod().SetLetter('t');
                AddBoldSteps("Hacemos a $$T(x)=(s2 - qx * s1)$$");
                AddSteps($"$$T(x)=({t2.ToString(false)}-({qx.ToString(false)}*{t1.ToString(false)}))$$");
                //11
                AddBoldSteps("Hacemos a $$G(x) = H(x)$$ , $$H(x) = R(x)$$");
                gx = hx.Clone().SetLetter('g');
                hx = rx.Clone().SetLetter('h');
                AddSteps(gx.ToLatexString(), hx.ToLatexString());
                //12
                AddBoldSteps("Hacemos a $$S_2 = S_1$$ , $$S_1 = S(x)$$ , $$T_2 = T_1$$ , $$T_1 = T(x)$$");
                s2 = s1.Clone().SetLetter("s_2");
                s1 = sx.Clone().SetLetter("s_1");
                t2 = t1.Clone().SetLetter("t_2");
                t1 = tx.Clone().SetLetter("t_1");
                AddSteps(s2.ToLatexString(), s1.ToLatexString(), t2.ToLatexString(), t1.ToLatexString());
            }
            AddBoldSteps($"{hx} es cero por lo tanto hemos terminado ...");
            //14
            Dx = gx;
            Sx = s2;
            Tx = t2;
            AddStepsBlock(-1);
            AddBoldSteps("Resultado");
            AddSteps(Dx.ToLatexString(), Hx.ToLatexString(), Sx.ToLatexString(), Tx.ToLatexString());
        }
    }
}

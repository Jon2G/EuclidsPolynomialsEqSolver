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

        public void Solve()
        {
            //por formula
            PolynomialEq gx = this.Gx.Clone();
            PolynomialEq hx = this.Hx.Clone();
            PolynomialEq s2 = new PolynomialEq('s', XTerm.One);
            PolynomialEq s1 = new PolynomialEq('s', XTerm.Zero);
            PolynomialEq t2 = new PolynomialEq('t', XTerm.Zero);
            PolynomialEq t1 = new PolynomialEq('t', XTerm.One);
            while (hx.IsNotZero)
            {
                //9
                PolynomialDivisionResult qxDiv = gx / hx;
                PolynomialEq qx = qxDiv.Result.Mod().SetLetter('q');
                PolynomialEq rx = qxDiv.Remainder.Mod().SetLetter('r');
                //10
                PolynomialEq sx = (s2 - (qx * s1)).Mod().SetLetter('s');
                PolynomialEq tx = (t2 - (qx * t1)).Mod().SetLetter('t');
                //11
                gx = hx.Clone().SetLetter('g');
                hx = rx.Clone().SetLetter('h');
                //12
                s2 = s1.Clone().SetLetter('s');
                s1 = sx.Clone().SetLetter('s');
                t2 = t1.Clone().SetLetter('t');
                t1 = tx.Clone().SetLetter('t');
            }
            //14
            Dx = gx;
            Sx = s2;
            Tx = t2;
        }

    }
}

using DiscreteMaths;

namespace EuclidsPolynomialsEqSolver
{
    public static class ConsoleTest
    {
        public static void Parse()
        {
            string value = "x^12+x^9+x^8+x^6+x^5+x^4+1";
            PolynomialEq.Parse(value, out PolynomialEq eq);
            Console.WriteLine(value);
            Console.WriteLine(eq.ToString());
        }

        public static void Run2()
        {
            //1 
            //x^{10} + x^9 + x^8 + x^6 + x^5 + x^4 + 1
            //x^9 + x^6 + x^5 + x^3 + x^2 + 1
            //2
            //x^12+x^9+x^8+x^6+x^5+x^4+1
            //x^ 8+ x^6+x^ 5+x ^3 +x^ 2+1
            //3
            //3x^2+4x-2
            //3x+1

            PolynomialEq.Parse("3x^2+4x-2", out PolynomialEq gx);
            PolynomialEq.Parse("3x+1", out PolynomialEq hx);


            ExtendedEuclides solution = new ExtendedEuclides(gx, hx);
            solution.Solve();
        }
        public static void Run()
        {
            PolynomialEq gx = new PolynomialEq('g',
                new XTerm(10),
                new XTerm(9),
                new XTerm(8),
                new XTerm(6),
                new XTerm(5),
                new XTerm(4),
                XTerm.One);

            PolynomialEq hx = new PolynomialEq('h',
                new XTerm(9),
                new XTerm(6),
                new XTerm(5),
                new XTerm(3),
                new XTerm(2),
                XTerm.One);

            ExtendedEuclides solution = new ExtendedEuclides(gx, hx);
            solution.Solve();




        }

        public static void Division()
        {
            //PolynomialEq fx = new PolynomialEq('f',
            //    new XTerm(8),
            //    new XTerm(7),
            //    new XTerm(6),
            //    new XTerm(2),
            //    XTerm.One);
            //PolynomialEq gx = new PolynomialEq('g',
            //    new XTerm(5),
            //    new XTerm(2),
            //    XTerm.X,
            //    XTerm.One);


            PolynomialEq fx = new PolynomialEq('f',
                new XTerm(9),
                new XTerm(6),
                new XTerm(5),
                new XTerm(3),
                new XTerm(2),
                XTerm.One);

            PolynomialEq gx = new PolynomialEq('g',
                new XTerm(8),
                new XTerm(7),
                new XTerm(6),
                new XTerm(2),
                XTerm.X);

            //PolynomialEq fx = new PolynomialEq('f',
            //    new XTerm(2, 2),
            //    new XTerm(1, -15),
            //    new XTerm(0, 25));
            //PolynomialEq gx = new PolynomialEq('g',
            //    XTerm.X,
            //    new XTerm(0, -5));

            Console.WriteLine(fx);
            Console.WriteLine(gx);

            PolynomialDivisionResult result = fx / gx;
            Console.WriteLine(result.Result.Mod());
            Console.WriteLine(result.Remainder.Mod().ToString());
        }

        public static void Sum()
        {
            PolynomialEq fx = new PolynomialEq('g',
                new XTerm(6),
                new XTerm(4),
                new XTerm(2),
                XTerm.X, XTerm.One);

            PolynomialEq hx = new PolynomialEq('h',
                new XTerm(7),
                XTerm.X, XTerm.One);

            Console.WriteLine(fx);
            Console.WriteLine(hx);

            PolynomialEq rx = fx + hx;
            Console.WriteLine(rx);
        }
        public static void Multitply()
        {

            //PolynomialEq fx = new PolynomialEq('g',
            //    new XTerm(7),
            //    new XTerm(5),
            //    new XTerm(4),
            //    new XTerm(3),
            //    XTerm.X,
            //    XTerm.One);

            //PolynomialEq hx = new PolynomialEq('h',
            //    new XTerm(3),
            //    XTerm.X,
            //    XTerm.One);

            PolynomialEq fx = new PolynomialEq('g',
                new XTerm(10),
                new XTerm(9),
                new XTerm(8),
                new XTerm(6),
                new XTerm(5),
                new XTerm(4),
                XTerm.One);

            PolynomialEq hx = new PolynomialEq('h',
                new XTerm(9),
                new XTerm(6),
                new XTerm(5),
                new XTerm(3),
                new XTerm(2),
                XTerm.X, XTerm.One);

            Console.WriteLine(fx);
            Console.WriteLine(hx);

            var rx = fx / hx;
            Console.WriteLine(rx.Remainder);
        }
    }
}

using DiscreteMaths;

namespace EuclidsPolynomialsEqSolver
{
    public static class ConsoleTest
    {
        public static void Run()
        {
            PolynomialEq gx = new PolynomialEq('g',
                new XTerm(10),
                new XTerm(9),
                new XTerm(8),
                new XTerm(6),
                new XTerm(5),
                new XTerm(4),
                new XTerm(0));

            PolynomialEq hx = new PolynomialEq('h',
                new XTerm(9),
                new XTerm(6),
                new XTerm(5),
                new XTerm(3),
                new XTerm(2),
                new XTerm(0));

            //por formula
            int s2 = 1;
            int s1 = 0;
            int t2 = 0;
            int t1 = 1;
            while (hx.IsNotZero)
            {
                PolynomialDivisionResult divisionResult = (gx / hx);


            }

            Console.WriteLine(gx);
            Console.WriteLine(hx);


        }
    }
}

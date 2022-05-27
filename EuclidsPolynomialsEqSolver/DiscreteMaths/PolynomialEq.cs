using Kit;
using System.Text;
namespace DiscreteMaths
{
    public class PolynomialEq : ICloneable<PolynomialEq>
    {
        public List<XTerm> Terms { get; set; }
        public char Letter { get; set; }
        public bool IsNotZero => !IsZero;
        public bool IsZero => false;
        public PolynomialEq(char letter, params XTerm[] xTerms)
        {
            SetLetter(letter);
            Terms = new List<XTerm>();
            if (xTerms?.Any() ?? false)
                Add(xTerms);
        }

        public PolynomialEq SetLetter(char letter)
        {
            Letter = letter;
            return this;
        }

        public static PolynomialEq operator -(PolynomialEq eq1, PolynomialEq eq2)
        {
            List<XTerm> toRest = eq2.Terms.ToArray().ToList();
            PolynomialEq result = new PolynomialEq('r', eq1.Terms.ToArray());
            //find if there are xterms with the same exponent in both equations
            foreach (XTerm term in eq1.Terms.Where(x => eq2.Terms.Any(y => y == x)))
            {
                //cancel found term
                result.Terms.Remove(term);
                toRest.Remove(term);
            }
            result.Add(toRest.ToArray()).SortTerms();
            return result;
        }
        public static PolynomialEq operator *(PolynomialEq eq1, PolynomialEq eq2)
        {
            PolynomialEq result = new PolynomialEq('m');
            for (int i = 0; i < eq1.Terms.Count; i++)
            {
                XTerm x1 = eq1.Terms[i];
                if (x1.Exponent == 0)
                    continue;
                for (int j = 0; j < eq2.Terms.Count; j++)
                {
                    XTerm x2 = eq2.Terms[j];
                    if (x2.Exponent == 0)
                        continue;
                    XTerm mult = x1 * x2;
                    result.Add(mult);
                }
            }
            return result;
        }
        public static PolynomialDivisionResult operator /(PolynomialEq eq1, PolynomialEq eq2)
        {
            //get first two terms and get difference between exponents
            XTerm term1 = eq1.Terms.First();
            XTerm term2 = eq2.Terms.First();
            int diff = term1.Exponent - term2.Exponent;
            if (diff <= 0)
            {
                throw new InvalidOperationException("PANIC");
            }

            PolynomialEq divisor = new PolynomialEq('d',
                new XTerm(diff, 1), //x^?
                                   new XTerm(0) //+ 1
            );

            PolynomialEq mult = eq2 * divisor;
            PolynomialEq r = eq1 - mult;

            return new PolynomialDivisionResult(divisor, r);
        }
        public PolynomialEq Add(params XTerm[] xTerms)
        {
            Terms.AddRange(xTerms);
            return this;
        }
        public PolynomialEq Add(XTerm xTerm)
        {
            Terms.Add(xTerm);
            return this;
        }

        public PolynomialEq SortTerms()
        {
            var sorted =
            this.Terms.OrderByDescending(x => x.Exponent).ThenByDescending(x => x.Value).ToArray();
            this.Terms.Clear();
            this.Add(sorted);
            return this;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{Letter}(x)=");
            foreach (XTerm term in Terms)
            {
                sb.Append($" {term} +");
            }
            return sb.RemoveLast().ToString();
        }

        public PolynomialEq Clone()
        {
            return new PolynomialEq(this.Letter, this.Terms.ToArray());
        }
    }
}
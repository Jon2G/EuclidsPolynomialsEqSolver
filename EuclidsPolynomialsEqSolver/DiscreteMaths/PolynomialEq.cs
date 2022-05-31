using Kit;
using System.Diagnostics;
using System.Text;
namespace DiscreteMaths
{
    [DebuggerDisplay("{ToString()},Count = {Count}")]
    public class PolynomialEq : List<XTerm>, ICloneable<PolynomialEq>
    {
        public char Letter { get; set; }
        public bool IsNotZero => !IsZero;
        public bool IsZero => this.All(x => x.IsZero);
        public PolynomialEq(char letter, params XTerm[] xTerms)
        {
            SetLetter(letter);
            if (xTerms?.Any() ?? false)
                Add(xTerms);
        }

        public PolynomialEq SetLetter(char letter)
        {
            Letter = letter;
            return this;
        }

        private static PolynomialEq SumRest(bool op, PolynomialEq eq1, PolynomialEq eq2)
        {
            PolynomialEq op1, op2;
            if (eq1.Count > eq2.Count)
            {
                op1 = eq1.Clone();
                op2 = eq2.Clone();
            }
            else
            {
                op1 = eq2.Clone();
                op2 = eq1.Clone();
            }
            if (!op)
            {
                eq2 = !eq2;
            }
            PolynomialEq result = new PolynomialEq('r');
            for (var i = 0; i < op1.Count; i++)
            {
                XTerm term1 = op1[i];
                for (var j = 0; j < op2.Count; j++)
                {
                    XTerm term2 = op2[j];
                    if (term1.Exponent == term2.Exponent)
                    {
                        XTerm sum = term1 + term2;
                        if (sum.IsNotZero)// sum != XTerm.Zero)
                        {
                            result.Add(sum);
                        }
                        i--;
                        j--;
                        op1.Remove(term1);
                        op2.Remove(term2);
                    }
                }
            }

            result.Add(op1.ToArray()).Add(op2.ToArray()).SortTerms();
            if (result.IsZero)
            {
                return new PolynomialEq('r', XTerm.Zero);
            }
            return result; //result.Mod();
        }

        public static PolynomialEq operator +(PolynomialEq eq1, PolynomialEq eq2) => SumRest(true, eq1, eq2);
        public static PolynomialEq operator -(PolynomialEq eq1, PolynomialEq eq2) => SumRest(false, eq1, eq2);

        public static PolynomialEq operator !(PolynomialEq eq)
        {
            PolynomialEq result = eq.Clone();
            foreach (var eq1 in result)
            {
                eq1.Value *= -1;
            }
            return result;
        }
        public static PolynomialEq operator *(PolynomialEq eq, XTerm term)
        {
            PolynomialEq result = new PolynomialEq('m');
            foreach (XTerm term1 in eq.ToArray().Reverse())
            {
                XTerm termMult = term1 * term;
                result.Prepend(termMult);
            }
            return result;
        }
        public static PolynomialEq operator *(PolynomialEq eq1, PolynomialEq eq2)
        {
            List<PolynomialEq> results = new List<PolynomialEq>();
            foreach (XTerm term1 in eq2.ToArray().Reverse())
            {
                PolynomialEq mresult = new PolynomialEq('m');
                foreach (XTerm term2 in eq1.ToArray().Reverse())
                {
                    XTerm termMult = term1 * term2;
                    mresult.Prepend(termMult);
                }
                results.Add(mresult);
            }

            PolynomialEq result = new PolynomialEq('m');
            foreach (PolynomialEq eq in results)
            {
                result += eq;
            }

            return result; //result.Mod();
        }

        public PolynomialEq Mod()
        {
            for (var index = 0; index < Count; index++)
            {
                XTerm term = this[index];
                int mod = Modulus.Mod(term.Value, 2);
                if (mod == 0)
                {
                    RemoveAt(index);
                    index--;
                }
                term.Value = mod;
            }

            return this;
        }

        public static PolynomialDivisionResult operator /(PolynomialEq eq1, PolynomialEq eq2)
        {
            eq1.SortTerms();
            eq2.SortTerms();
            //get first two terms and get difference between exponents
            XTerm term1 = eq1.First();
            XTerm term2 = eq2.First();

            int diffValues = term1.Value / term2.Value;
            int diffExponents = Math.Abs(term1.Exponent - term2.Exponent);

            XTerm d1 = new XTerm(diffExponents, diffValues);
            if (d1 * term2 != term1)
            {
                return new PolynomialDivisionResult(new PolynomialEq('d', XTerm.Zero), eq1);
            }

            PolynomialEq divisor = new PolynomialEq('d', d1);
            PolynomialEq mult = eq2 * divisor;
            PolynomialEq r = (eq1 - mult).SetLetter('r');

            List<PolynomialEq> divisors = new List<PolynomialEq>(new[] { divisor });
            while (r.IsNotZero)
            {
                var recursive = r / eq2;
                r = recursive.Remainder;
                divisors.Add(recursive.Result);
                if (recursive.Result.IsZero) break;
            }
            XTerm[] terms = divisors.SelectMany(x => x.Where(x => x != XTerm.Zero).ToArray()).ToArray();
            divisor = new PolynomialEq('d', terms);
            mult = eq2 * divisor;
            r = (eq1 - mult).SetLetter('r');

            return new PolynomialDivisionResult(divisor, r);
        }
        public PolynomialEq Add(params XTerm[] xTerms)
        {
            this.AddRange(xTerms);
            return this;
        }

        public PolynomialEq SortTerms()
        {
            var sorted =
            this.OrderByDescending(x => x.Exponent).ThenByDescending(x => x.Value).ToArray();
            this.Clear();
            this.Add(sorted);
            return this;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{Letter}(x)=");
            if (Count <= 0)
            {
                return sb.Append("0").ToString();
            }
            for (var i = 0; i < Count; i++)
            {
                XTerm term = this[i];
                if (i == 0 && term.Sign)
                {
                    sb.Append(term.ToString().Substring(2)).Append(" ");
                    continue;
                }
                sb.Append(term).Append(" ");
            }

            return sb.RemoveLast().ToString();
        }

        public PolynomialEq Clone()
        {
            return new PolynomialEq(this.Letter, this.ToArray());
        }

        object ICloneable.Clone() => this.Clone();
    }
}
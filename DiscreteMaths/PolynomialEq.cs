using Kit;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscreteMaths
{
    [DebuggerDisplay("{ToString()},Count = {Count}")]
    public class PolynomialEq : List<XTerm>, ICloneable<PolynomialEq>
    {
        public string Letter { get; set; }
        public bool IsNotZero => !IsZero;
        public bool IsZero => this.All(x => x.IsZero);

        public static readonly Regex EquationExpression = new Regex(@"(?<Equation>(?<NumericTerm>(?<Sign>\+|\-)(?<Value>\d+))|(?<XTerm>(?<Sign>\+|\-)?(?<Value>\d)?x(\^\{?(?<Exponent>\d+)\}?)?))+");
        public static readonly Regex NumericTermExpression = new Regex(@"(?<NumericTerm>(?<Sign>\+|\-)(?<Value>\d+))");
        public static readonly Regex XTermExpression = new Regex(@"(?<XTerm>(?<Sign>\+|\-)?(?<Value>\d)?x(\^\{?(?<Exponent>\d+)\}?)?)");

        public PolynomialEq(string letter, params XTerm[] xTerms)
        {
            SetLetter(letter);
            if (xTerms?.Any() ?? false)
                Add(xTerms);
        }
        public PolynomialEq(char letter, params XTerm[] xTerms) : this(letter.ToString(), xTerms)
        {

        }

        public PolynomialEq SetLetter(char letter) => this.SetLetter(letter.ToString());
        public PolynomialEq SetLetter(string letter)
        {
            Letter = letter.ToUpper();
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
                        if (i < -1) i = 0;
                        if (j < -1) j = 0;
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
                int mod = Kit.Security.Encryption.Modulus.Mod(term.Value, 2);
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

        public string ToString(bool addLetter)
        {
            StringBuilder sb = new StringBuilder(addLetter ? $"{Letter}(x)=" : "");
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

        public override string ToString() => ToString(true);
        public MarkupString ToLatexString(bool addLetter = true) => (MarkupString)$"$${ToString(addLetter)}$$";
        public PolynomialEq Clone()
        {
            return new PolynomialEq(this.Letter, this.ToArray());
        }

        object ICloneable.Clone() => this.Clone();

        public static bool Parse(string value, out PolynomialEq polynomialEq)
        {
            polynomialEq = new PolynomialEq('p');
            value = value?.Trim()?.ToLower()?.Replace(" ", "");
            if (string.IsNullOrEmpty(value)) return false;
            Match match = EquationExpression.Match(value);
            if (!match.Success)
            {
                return false;
            }
            Group equationGroup = match.Groups["Equation"];
            foreach (Capture capture in equationGroup.Captures)
            {
                int numericValue = 1;
                int exponent = 0;
                match = NumericTermExpression.Match(capture.Value);

                Group s, v;

                if (match.Success)
                {
                    v = match.Groups["Value"];
                    if (!string.IsNullOrEmpty(v.Value))
                    {
                        numericValue = int.Parse(v.Value);
                    }
                }
                else
                {
                    match = XTermExpression.Match(capture.Value);
                    if (!match.Success) return false;

                    v = match.Groups["Value"];
                    if (!string.IsNullOrEmpty(v.Value))
                    {
                        numericValue = int.Parse(v.Value);
                    }
                    var e = match.Groups["Exponent"];
                    if (!string.IsNullOrEmpty(e.Value))
                    {
                        exponent = int.Parse(e.Value);
                    }
                }
                s = match.Groups["Sign"];
                if (s.Value == "-")
                {
                    numericValue = -1 * numericValue;
                }
                XTerm term = new XTerm(exponent, numericValue);
                polynomialEq.Add(term);
            }
            return true;
        }
    }
}
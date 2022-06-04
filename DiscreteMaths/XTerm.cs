namespace DiscreteMaths
{
    public class XTerm : IEquatable<XTerm>, IComparable, IComparable<XTerm>
    {
        public int Exponent { get; set; }
        public int Value { get; set; }
        public bool Sign => Value >= 0;
        public bool Unitary => Math.Abs(Value) == 1;
        public static XTerm X => new XTerm(1);
        public static XTerm One => new XTerm(0, 1);
        public static XTerm Zero => new XTerm(0, 0);
        public bool IsZero => Value == 0;
        public bool IsNotZero => !IsZero;
        public XTerm(int exponent, int value = 1)
        {
            Exponent = exponent;
            Value = value;
        }

        public static bool operator !=(XTerm? eq1, XTerm? eq2) => !(eq1 == eq2);
        public static bool operator ==(XTerm? eq1, XTerm? eq2)
        {
            if (eq1 is null && eq2 is { }) return false;
            if (eq2 is null && eq1 is { }) return false;
            if (eq1 is null && eq2 is null) return true;
            return eq1.Exponent == eq2.Exponent && eq1.Value == eq2.Value;
        }

        public static XTerm SumRest(bool op, XTerm term1, XTerm term2)
        {
            if (term1.Exponent != term1.Exponent)
                throw new InvalidOperationException("No se pueden sumar terminos con exponentes distintos");
            int value = op ? term1.Value + term2.Value : term1.Value - term2.Value;
            if (value == 0) return Zero;
            return new XTerm(term1.Exponent, value);
        }

        public static XTerm operator -(XTerm term1, XTerm term2) => SumRest(false, term1, term2);
        public static XTerm operator +(XTerm term1, XTerm term2) => SumRest(true, term1, term2);
        public static XTerm operator *(XTerm eq1, XTerm eq2)
        {
            int values = eq1.Value * eq2.Value;
            int exponents = eq1.Exponent + eq2.Exponent;
            return new XTerm(exponents, values);
        }

        public bool Equals(XTerm? other) => other == this;
        public int CompareTo(XTerm? other) => other == this ? 0 : 1;

        public override string ToString()
        {
            string text;
            switch (Exponent)
            {
                case 1 when Unitary:
                    text = "x";
                    break;
                case 0:
                    text = Math.Abs(Value).ToString();
                    break;
                default:
                    {
                        string textExponent = Exponent.ToString();
                        if (textExponent.Length > 1)
                        {
                            textExponent = $"{{{textExponent}}}";
                        }
                        if (Unitary)
                        {
                            text = $"x^{textExponent}";
                        }
                        else if (Exponent == 1)
                        {
                            text = $"{Math.Abs(Value)}x";
                        }
                        else
                        {
                            text = $"{Math.Abs(Value)}x^{textExponent}";
                        }
                        break;
                    }
            }
            text = $"{(Sign ? "+" : "-")} {text}";
            return text;
        }

        public int CompareTo(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}

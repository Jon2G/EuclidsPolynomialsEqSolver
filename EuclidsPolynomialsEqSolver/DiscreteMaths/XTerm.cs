namespace DiscreteMaths
{
    public class XTerm : IEquatable<XTerm>, IComparable, IComparable<XTerm>
    {
        public int Exponent { get; set; }
        public int Value { get; set; }

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
            if (Exponent == 1 && Value == 1)
                return "x";
            if (Exponent == 0)
                return Value.ToString();
            if (Value == 1)
                return $"x^{Exponent}";
            if (Exponent == 1)
                return $"{Value}x";
            return $"{Value}x^{Exponent}";
        }

        public int CompareTo(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}

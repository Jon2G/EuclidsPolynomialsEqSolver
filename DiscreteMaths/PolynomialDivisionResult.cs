namespace DiscreteMaths
{
    public class PolynomialDivisionResult
    {
        public readonly PolynomialEq Result;
        public readonly PolynomialEq Remainder;
        public PolynomialDivisionResult(PolynomialEq result, PolynomialEq remainder)
        {
            Result = result;
            Remainder = remainder;
        }
    }
}

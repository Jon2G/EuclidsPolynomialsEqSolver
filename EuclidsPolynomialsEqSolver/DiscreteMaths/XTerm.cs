namespace DiscreteMaths
{
    public class XTerm
    {
        public int Exponent { get; set; }
        public int Value { get; set; }

        public XTerm()
        {

        }

        public override string ToString()
        {
            return $"{Value}x^{Exponent}";
        }
    }
}

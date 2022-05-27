using Kit;
using System.Text;
namespace DiscreteMaths
{
    public class PolynomialEq
    {
        public XTerm[] Terms { get; set; }
        public PolynomialEq()
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (XTerm term in Terms)
            {
                sb.Append($" {term} +");
            }
            return sb.RemoveLast().ToString();
        }
    }
}
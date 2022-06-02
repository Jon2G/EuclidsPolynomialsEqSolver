using Microsoft.AspNetCore.Components;

namespace DiscreteMaths
{
    public class Step : List<MarkupString>
    {
        public int Number { get; set; }

        public string GetNumber()
        {
            if (Number == -1)
                return "R";
            if (Number > 0)
                return Number.ToString();
            return string.Empty;
        }
        public Step()
        {

        }
    }
}

namespace DiscreteMaths
{
    public class SolutionStep<T> where T : ICloneable
    {
        public T Value { get; set; }
        public SolutionStep(T value)
        {

        }
    }
}

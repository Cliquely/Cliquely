namespace BacteriaNetworks.Infrastructure
{
    public class BacteriaProbabilityOption
    {
        public string Text { get; }
        public float Value { get; }

        public BacteriaProbabilityOption(string text, float value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

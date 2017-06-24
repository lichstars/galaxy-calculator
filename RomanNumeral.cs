namespace GuideToTheGalaxy
{
    public class RomanNumeral
    {
        public RomanNumeral(char symbol, double value, bool repeatable)
        {
            Symbol = symbol;
            Value = value;
            CanRepeat = repeatable;
        }
        public char Symbol { get; private set; }
        public double Value { get; private set; }
        public bool CanRepeat { get; private set; }
    }
}

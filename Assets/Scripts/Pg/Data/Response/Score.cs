#nullable enable
namespace Pg.Data.Response
{
    public readonly struct Score
    {
        public static Score Zero { get; } = new Score(value: 0);

        int Value { get; }

        public Score(int value)
        {
            Value = value;
        }

        public Score Add(AcquisitionScore acquisitionScore)
        {
            return new Score(Value + acquisitionScore.Value);
        }

        public string GetText()
        {
            return Value.ToString();
        }

        public int GetValue()
        {
            return Value;
        }

        public bool IsGreaterThanEqual(Score other)
        {
            return Value >= other.Value;
        }

        public override string ToString()
        {
            return GetText();
        }
    }
}

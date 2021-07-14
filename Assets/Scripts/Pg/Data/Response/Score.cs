#nullable enable
namespace Pg.Data.Response
{
    public readonly struct Score
    {
        public Score(int value)
        {
            Value = value;
        }

        int Value { get; }
        public static Score Zero { get; } = new Score(value: 0);

        public string GetText()
        {
            return Value.ToString();
        }

        public override string ToString()
        {
            return GetText();
        }

        public Score Add(Score other)
        {
            return new Score(Value + other.Value);
        }

        public bool IsGreaterThanEqual(Score other)
        {
            return Value >= other.Value;
        }
    }
}

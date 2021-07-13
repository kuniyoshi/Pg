#nullable enable
namespace Pg.Data
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
    }
}

#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalScore
    {
        public static TotalScore Get(int value)
        {
            return new TotalScore(value);
        }

        int Value { get; }

        TotalScore(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TotalScore other)
            {
                return other.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public int GetValue()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

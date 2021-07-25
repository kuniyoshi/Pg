#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct TargetScore
    {
        public static TargetScore Get(int value)
        {
            return new TargetScore(value);
        }

        int Value { get; }

        TargetScore(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TargetScore other)
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

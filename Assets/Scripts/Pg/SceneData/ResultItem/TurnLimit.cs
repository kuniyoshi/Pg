#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct TurnLimit
    {
        public static TurnLimit Get(int value)
        {
            return new TurnLimit(value);
        }

        int Value { get; }

        TurnLimit(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TurnLimit other)
            {
                return other.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

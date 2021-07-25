#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalTurn
    {
        public static TotalTurn Get(int value)
        {
            return new TotalTurn(value);
        }

        int Value { get; }

        TotalTurn(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TotalTurn other)
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

#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalChain
    {
        public static TotalChain Get(int value)
        {
            return new TotalChain(value);
        }

        int Value { get; }

        TotalChain(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TotalChain other)
            {
                return other.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}

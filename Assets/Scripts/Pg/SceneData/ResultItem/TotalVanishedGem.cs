#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalVanishedGem
    {
        public static TotalVanishedGem Get(int value)
        {
            return new TotalVanishedGem(value);
        }

        int Value { get; }

        TotalVanishedGem(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is TotalVanishedGem other)
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

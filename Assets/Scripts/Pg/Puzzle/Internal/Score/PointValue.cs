#nullable enable
namespace Pg.Puzzle.Internal.Score
{
    public readonly struct PointValue
    {
        internal static PointValue CreateVanished(int clusterSize, int lastChainCount)
        {
            return new PointValue(clusterSize * 10 + lastChainCount * clusterSize * 10);
        }

        internal static PointValue Zero { get; } = new PointValue();

        int Value { get; }

        PointValue(int value)
        {
            Value = value;
        }

        public PointValue Add(PointValue other)
        {
            return new PointValue(Value + other.Value);
        }

        public string GetText()
        {
            return Value.ToString();
        }
    }
}

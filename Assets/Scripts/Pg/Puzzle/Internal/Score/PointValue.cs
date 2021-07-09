#nullable enable
namespace Pg.Puzzle.Internal.Score
{
    internal readonly struct PointValue
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

        internal PointValue Add(PointValue other)
        {
            return new PointValue(Value + other.Value);
        }

        internal string GetText()
        {
            return Value.ToString();
        }
    }
}

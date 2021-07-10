#nullable enable
namespace Pg.Data
{
    public readonly struct PointValue
    {
        public static PointValue CreateVanished(int clusterSize, int lastChainCount)
        {
            return new PointValue(clusterSize * 10 + lastChainCount * clusterSize * 10);
        }

        public static PointValue Zero { get; } = new PointValue();

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

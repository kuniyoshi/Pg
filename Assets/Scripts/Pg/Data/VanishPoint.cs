#nullable enable
namespace Pg.Data
{
    public readonly struct VanishPoint
    {
        public static VanishPoint Zero { get; } = new VanishPoint();

        int Value { get; }

        public VanishPoint(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is VanishPoint vanishPoint && Equals(vanishPoint);
        }

        public bool Equals(VanishPoint? other)
        {
            return Value == other?.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public VanishPoint Add(VanishPoint other)
        {
            return new VanishPoint(Value + other.Value);
        }

        public string GetText()
        {
            return Value.ToString();
        }
    }
}

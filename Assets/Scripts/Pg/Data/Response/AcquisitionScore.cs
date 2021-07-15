#nullable enable
namespace Pg.Data.Response
{
    public readonly struct AcquisitionScore
    {
        public int Value { get; }

        public AcquisitionScore(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is AcquisitionScore acquisitionScore && Equals(acquisitionScore);
        }

        public bool Equals(AcquisitionScore? other)
        {
            return Value == other?.Value;
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

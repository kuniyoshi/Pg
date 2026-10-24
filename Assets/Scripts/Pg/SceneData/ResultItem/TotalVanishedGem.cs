#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalVanishedGem : IEquatable<TotalVanishedGem>
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
            return obj is TotalVanishedGem other && Equals(other);
        }

        public bool Equals(TotalVanishedGem other)
        {
            return Value == other.Value;
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

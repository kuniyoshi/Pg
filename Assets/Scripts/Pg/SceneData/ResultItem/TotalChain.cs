#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalChain : IEquatable<TotalChain>
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
            return obj is TotalChain other && Equals(other);
        }

        public bool Equals(TotalChain other)
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

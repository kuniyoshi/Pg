#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalScore : IEquatable<TotalScore>
    {
        public static TotalScore Get(int value)
        {
            return new TotalScore(value);
        }

        int Value { get; }

        TotalScore(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TotalScore other && Equals(other);
        }

        public bool Equals(TotalScore other)
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

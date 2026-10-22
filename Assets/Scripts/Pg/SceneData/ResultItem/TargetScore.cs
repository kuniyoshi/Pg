#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    public readonly struct TargetScore : IEquatable<TargetScore>
    {
        public static TargetScore Get(int value)
        {
            return new TargetScore(value);
        }

        int Value { get; }

        TargetScore(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TargetScore other && Equals(other);
        }

        public bool Equals(TargetScore other)
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

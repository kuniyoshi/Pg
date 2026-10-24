#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    public readonly struct TurnLimit : IEquatable<TurnLimit>
    {
        public static TurnLimit Get(int value)
        {
            return new TurnLimit(value);
        }

        int Value { get; }

        TurnLimit(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TurnLimit other && Equals(other);
        }

        public bool Equals(TurnLimit other)
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

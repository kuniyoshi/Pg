#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    public readonly struct TotalTurn : IEquatable<TotalTurn>
    {
        public static TotalTurn Get(int value)
        {
            return new TotalTurn(value);
        }

        int Value { get; }

        TotalTurn(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TotalTurn other && Equals(other);
        }

        public bool Equals(TotalTurn other)
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

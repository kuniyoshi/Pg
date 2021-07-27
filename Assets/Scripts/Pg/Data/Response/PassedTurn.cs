#nullable enable
using System.Linq;
using Pg.Etc.Puzzle;
using UnityEngine.Assertions;

namespace Pg.Data.Response
{
    public readonly struct PassedTurn
    {
        static PassedTurn[] DomainValues { get; } = Enumerable
            .Range(start: 0, SimulationLimitation.MaxProcessedTurnExclusive)
            .Select(index => new PassedTurn(index))
            .ToArray();

        public static PassedTurn Get(int value)
        {
            Assert.IsTrue(value >= 0 && value < DomainValues.Length, "value >= 0 && value < DomainValues.Length");
            Assert.IsTrue(
                value < DomainValues.Length / 2,
                "Chaining count almost reaches max limitation, consider increase it, ot reduce chain"
            );
            return DomainValues[value];
        }

        int Value { get; }

        public PassedTurn(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is PassedTurn processedTurn && Equals(processedTurn);
        }

        public bool Equals(PassedTurn? other)
        {
            return Value == other?.Value;
        }

        public float GetCoef()
        {
            return 0.1f * (Value + 1);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public PassedTurn Increment()
        {
            return new PassedTurn(Value + 1);
        }

        public bool IsGreaterThanEqual(PassedTurn other)
        {
            return Value >= other.Value;
        }
    }
}

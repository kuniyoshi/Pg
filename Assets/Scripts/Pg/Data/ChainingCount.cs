#nullable enable
using System.Linq;
using Pg.Etc.Puzzle;
using UnityEngine.Assertions;

namespace Pg.Data
{
    public readonly struct ChainingCount
    {
        static ChainingCount[] DomainValues { get; } = Enumerable.Range(0, SimulationLimitation.MaxChainingCountExclusive)
            .Select(index => new ChainingCount(index))
            .ToArray();

        public static ChainingCount Get(int value)
        {
            Assert.IsTrue(value >= 0 && value < DomainValues.Length, "value >= 0 && value < DomainValues.Length");
            Assert.IsTrue(
                value < DomainValues.Length / 2,
                "Chaining count almost reaches max limitation, consider increase it, ot reduce chain"
            );
            return DomainValues[value];
        }

        int Value { get; }

        ChainingCount(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is ChainingCount chainingCount && Equals(chainingCount);
        }

        public bool Equals(ChainingCount? other)
        {
            return Value == other?.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}

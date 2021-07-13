#nullable enable
using System.Linq;
using Pg.Etc.Puzzle;
using UnityEngine.Assertions;

namespace Pg.Data.Response
{
    public readonly struct ClusterSize
    {
        static ClusterSize[] DomainValues { get; } = Enumerable.Range(start: 0, TileSize.ColSize * TileSize.RowSize)
            .Select(index => new ClusterSize(index))
            .ToArray();

        public static ClusterSize Get(int value)
        {
            Assert.IsTrue(value >= 0 && value < DomainValues.Length, "value >= 0 && value < DomainValues.Length");
            return DomainValues[value];
        }

        public int Value { get; }

        public static bool operator ==(ClusterSize? a, ClusterSize? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            return a?.Equals(b) ?? false;
        }

        public static bool operator !=(ClusterSize? a, ClusterSize? b)
        {
            return !(a == b);
        }

        public ClusterSize(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is ClusterSize clusterSize && Equals(clusterSize);
        }

        public bool Equals(ClusterSize? other)
        {
            return Value == other?.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}

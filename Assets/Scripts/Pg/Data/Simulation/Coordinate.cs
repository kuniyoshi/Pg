#nullable enable
using System;

namespace Pg.Data.Simulation
{
    public readonly struct Coordinate : IEquatable<Coordinate>
    {
        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return !a.Equals(b);
        }

        public int Column { get; }
        public int Row { get; }

        public Coordinate(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override bool Equals(object? obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public bool Equals(Coordinate other)
        {
            return Column == other.Column
                   && Row == other.Row;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }

        public override string ToString()
        {
            return $"({Column}, {Row})";
        }
    }
}

#nullable enable
namespace Pg.Puzzle
{
    public readonly struct Coordinate
    {
        public override bool Equals(object? obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Column * 397) ^ Row;
            }
        }

        public int Column { get; }
        public int Row { get; }

        public Coordinate(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public bool Equals(Coordinate other)
        {
            return Column == other.Column
                   && Row == other.Row;
        }

        public static bool operator ==(Coordinate a, Coordinate b)
        {
            return a.Column == b.Column && a.Row == b.Row;
        }
        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return a.Column != b.Column || a.Row != b.Row;
        }

        public override string ToString()
        {
            return $"({Column}, {Row})";
        }
    }
}

#nullable enable
namespace Pg.Puzzle
{
    public readonly struct Coordinate
    {
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

        public override string ToString()
        {
            return $"({Column}, {Row})";
        }
    }
}

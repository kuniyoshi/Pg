#nullable enable
using Pg.Data;

namespace Pg.Puzzle.Request
{
    public readonly struct TileOperation
    {
        public Coordinate A { get; }
        public Coordinate B { get; }

        public TileOperation(Coordinate a, Coordinate b)
        {
            A = a;
            B = b;
        }
    }
}

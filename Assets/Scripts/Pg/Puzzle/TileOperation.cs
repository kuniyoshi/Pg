#nullable enable
namespace Pg.Puzzle
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

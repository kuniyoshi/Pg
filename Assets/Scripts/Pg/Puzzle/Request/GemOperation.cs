#nullable enable
using Pg.Data.Simulation;

namespace Pg.Puzzle.Request
{
    public readonly struct GemOperation
    {
        public Coordinate A { get; }
        public Coordinate B { get; }

        public GemOperation(Coordinate a, Coordinate b)
        {
            A = a;
            B = b;
        }
    }
}

#nullable enable
using Pg.Data.Simulation;
using Pg.Puzzle.Internal;

namespace Pg.Puzzle
{
    public static class GameRule
    {
        public static bool CanSwap(Coordinate a, Coordinate b)
        {
            return DirectionService.IsNeighborEachOther(a, b);
        }
    }
}

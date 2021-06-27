#nullable enable
using Pg.Puzzle.Internal;

namespace Pg.Puzzle
{
    public static class GameRule
    {
        public static bool CanSwap(TileData a, TileData b)
        {
            return TileStatusService.CanBothBeSwappable(a.TileStatus, b.TileStatus)
                   && DirectionService.IsNeighborEachOther(a.Coordinate, b.Coordinate);
        }
    }
}

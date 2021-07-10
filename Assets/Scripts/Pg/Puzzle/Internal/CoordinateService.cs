#nullable enable
using Pg.Data;

namespace Pg.Puzzle.Internal
{
    internal static class CoordinateService
    {
        internal static bool IsCoordinateInRange(Coordinate coordinate, TileStatus[,] array)
        {
            return coordinate.Column >= 0
                   && coordinate.Column < array.GetLength(dimension: 0)
                   && coordinate.Row >= 0
                   && coordinate.Row < array.GetLength(dimension: 1);
        }

        internal static bool IsTopRow(Coordinate valueTo)
        {
            return valueTo.Row == 0;
        }
    }
}

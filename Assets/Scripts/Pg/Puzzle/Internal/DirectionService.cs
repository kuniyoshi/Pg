#nullable enable
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    /// <summary>
    ///     even q directions
    /// </summary>
    internal static class DirectionService
    {
        internal static Coordinate GetNeighborOf(Coordinate coordinate, int neighborIndex)
        {
            Assert.IsTrue(
                neighborIndex >= 0 && neighborIndex < NeighborDirections.GetLength(dimension: 1),
                "neighborIndex >= 0 && neighborIndex < NeighborDirections.GetLength(1)"
            );
            var party = coordinate.Column & 1;
            var direction = NeighborDirections[party, neighborIndex];

            return new Coordinate(
                coordinate.Column + direction.Column,
                coordinate.Row + direction.Row
            );
        }

        internal static bool IsNeighborEachOther(Coordinate a, Coordinate b)
        {
            var directionSize = NeighborDirections.GetLength(dimension: 1);

            for (var i = 0; i < directionSize; ++i)
            {
                if (a == GetNeighborOf(b, i))
                {
                    return true;
                }
            }

            return false;
        }

        internal static int NeighborSize => NeighborDirections.GetLength(dimension: 1);

        static Coordinate[,] NeighborDirections { get; } =
        {
            {
                new Coordinate(column: 1, row: 1),
                new Coordinate(column: 1, row: 0),
                new Coordinate(column: 0, row: -1),
                new Coordinate(column: -1, row: 0),
                new Coordinate(column: -1, row: 1),
                new Coordinate(column: 0, row: 1),
            },
            {
                new Coordinate(column: 1, row: 0),
                new Coordinate(column: 1, row: -1),
                new Coordinate(column: 0, row: -1),
                new Coordinate(column: -1, row: -1),
                new Coordinate(column: -1, row: 0),
                new Coordinate(column: 0, row: 1),
            },
        };
    }
}

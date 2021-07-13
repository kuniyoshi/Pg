#nullable enable
using Pg.Data.Simulation;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    /// <summary>
    ///     even q directions
    /// </summary>
    internal static class DirectionService
    {
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

        internal static Coordinate GetBelow(Coordinate above)
        {
            return new Coordinate(above.Column, above.Row + 1);
        }

        internal static Coordinate GetJustAbove(Coordinate coordinate)
        {
            const int aboveIndex = 2;
            return GetNeighborOf(coordinate, aboveIndex);
        }

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

        internal static Coordinate GetUpperLeft(Coordinate coordinate)
        {
            const int upperLeftIndex = 3;
            return GetNeighborOf(coordinate, upperLeftIndex);
        }

        internal static Coordinate GetUpperRight(Coordinate coordinate)
        {
            const int upperRightIndex = 1;
            return GetNeighborOf(coordinate, upperRightIndex);
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
    }
}

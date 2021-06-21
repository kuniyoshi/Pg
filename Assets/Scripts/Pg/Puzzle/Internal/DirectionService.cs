using UnityEngine.Assertions;

#nullable enable
namespace Pg.Puzzle.Internal
{
    /// <summary>
    /// even q directions
    /// </summary>
    internal static class DirectionService
    {
        static Coordinate[,] NeighborDirections { get; } = {
            {
                new Coordinate(1, 1),
                new Coordinate(1, 0),
                new Coordinate(0, -1),
                new Coordinate(-1, 0),
                new Coordinate(-1, 1),
                new Coordinate(0, 1),
            },
            {
                new Coordinate(1, 0),
                new Coordinate(1, -1),
                new Coordinate(0, -1),
                new Coordinate(-1, -1),
                new Coordinate(-1, 0),
                new Coordinate(0, 1),
            },
        };

        public static Coordinate GetNeighborOf(Coordinate coordinate, int neighborIndex)
        {
            Assert.IsTrue(
                neighborIndex >= 0 && neighborIndex < NeighborDirections.GetLength(1),
                "neighborIndex >= 0 && neighborIndex < NeighborDirections.GetLength(1)"
            );
            var party = coordinate.Column & 1;
            var direction = NeighborDirections[party, neighborIndex];

            return new Coordinate(
                coordinate.Column + direction.Column,
                coordinate.Row + direction.Row
            );
        }

        public static bool IsNeighborEachOther(Coordinate a, Coordinate b)
        {
            var directionSize = NeighborDirections.GetLength(1);

            for (var i = 0; i < directionSize; ++i)
            {
                if (a.Equals(GetNeighborOf(b, i)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

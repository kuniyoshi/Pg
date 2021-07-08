#nullable enable
using Pg.Etc.Puzzle;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class Map
    {
        internal static Map CreateMap(TileStatus[,] tileStatuses)
        {
            var colSize = tileStatuses.GetLength(dimension: 0);
            var rowSize = tileStatuses.GetLength(dimension: 1);
            var map = new TileStatus[colSize, rowSize];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    map[colIndex, rowIndex] = tileStatuses[colIndex, rowIndex];
                }
            }

            return new Map(map);
        }

        internal TileStatus[,] CurrentTileStatuses { get; }

        internal Map(TileStatus[,] currentTileStatuses)
        {
            CurrentTileStatuses = currentTileStatuses;
        }

        internal void AddGem(Coordinate toCoordinate, GemColorType gemColorType)
        {
            Assert.AreEqual(TileStatus.Empty, GetTileStatusAt(toCoordinate));
            CurrentTileStatuses[toCoordinate.Column, toCoordinate.Row] = new TileStatus(
                TileStatusType.Contain,
                gemColorType
            );
        }

        internal TileStatus GetTileStatusAt(Coordinate atCoordinate)
        {
            Assert.IsTrue(
                CoordinateService.IsCoordinateInRange(atCoordinate, CurrentTileStatuses),
                "CoordinateService.IsCoordinateInRange(atCoordinate, CurrentTileStatuses)"
            );
            return CurrentTileStatuses[atCoordinate.Column, atCoordinate.Row];
        }

        internal bool HasTileStatusContain(Coordinate coordinate, GemColorType gemColorType)
        {
            return CurrentTileStatuses[coordinate.Column, coordinate.Row].GemColorType == gemColorType;
        }

        internal bool IsCoordinateInRange(Coordinate neighbor)
        {
            return CoordinateService.IsCoordinateInRange(neighbor, CurrentTileStatuses);
        }

        internal void SetEmpty(Coordinate coordinate)
        {
            CurrentTileStatuses[coordinate.Column, coordinate.Row] = TileStatus.Empty;
        }

        internal void Swap(Coordinate a, Coordinate b)
        {
            (CurrentTileStatuses[a.Column, a.Row], CurrentTileStatuses[b.Column, b.Row]) = (
                CurrentTileStatuses[b.Column, b.Row], CurrentTileStatuses[a.Column, a.Row]);
        }
    }
}

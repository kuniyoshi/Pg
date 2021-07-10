#nullable enable
using System.Collections.Generic;
using System.Linq;
using Pg.Data;
using Pg.Etc.Puzzle;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class Map
    {
        static IEnumerable<Coordinate> GetCoordinates(TileStatus[,] map)
        {
            return Enumerable.Range(start: 0, map.GetLength(dimension: 0))
                .SelectMany(
                    colIndex =>
                    {
                        return Enumerable.Range(start: 0, map.GetLength(dimension: 1))
                            .Select(rowIndex => new Coordinate(colIndex, rowIndex));
                    }
                );
        }

        internal TileStatus[,] CurrentTileStatuses { get; }

        internal Map(TileStatus[,] tileStatuses)
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

            CurrentTileStatuses = map;
        }

        internal void AddGem(Coordinate toCoordinate, GemColorType gemColorType)
        {
            Assert.AreEqual(TileStatus.Empty, GetTileStatusAt(toCoordinate));
            CurrentTileStatuses[toCoordinate.Column, toCoordinate.Row] = new TileStatus(
                TileStatusType.Contain,
                gemColorType
            );
        }

        internal TileStatus[,] Clone()
        {
            var clone = new TileStatus[CurrentTileStatuses.GetLength(dimension: 0),
                CurrentTileStatuses.GetLength(dimension: 1)];

            foreach (var coordinate in GetCoordinates(CurrentTileStatuses))
            {
                clone[coordinate.Column, coordinate.Row] = CurrentTileStatuses[coordinate.Column, coordinate.Row];
            }

            return clone;
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

        internal bool IsEmptyAt(Coordinate coordinate)
        {
            return GetTileStatusAt(coordinate).TileStatusType == TileStatusType.Empty;
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

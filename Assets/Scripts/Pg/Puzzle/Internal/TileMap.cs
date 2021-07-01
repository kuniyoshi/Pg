#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class TileMap
    {
        static NewSlidingGems NewSlidingGems(VanishingClusters vanishingClusters)
        {
            var rowCountOf = new Dictionary<int, int>();

            foreach (var gemColorType in vanishingClusters.GemColorTypes)
            {
                foreach (var coordinateList in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    foreach (var coordinate in coordinateList)
                    {
                        if (rowCountOf.ContainsKey(coordinate.Column))
                        {
                            rowCountOf[coordinate.Column]++;
                        }
                    }
                }
            }

            var newCoordinates = rowCountOf.Keys
                .Select(column => new Coordinate(column, row: -1))
                .Select(top => Enumerable.Range(start: 0, rowCountOf[top.Column])
                    .Aggregate(top, (coordinate, _) => DirectionService.GetBelow(coordinate))
                );

            return new NewSlidingGems(newCoordinates);
        }

        internal TileMap(TileStatus[,] tileStatuses)
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

        internal TileStatus[,] CurrentTileStatuses { get; }

        internal SimulationStepData ProcessTurn()
        {
            var clusters = DetectClusters();
            var vanishingClusters = new VanishingClusters(clusters);
            MakeClustersVanish(vanishingClusters);
            var slidingGems = SlideGems(vanishingClusters);
            var newGems = NewSlidingGems(vanishingClusters);

            return new SimulationStepData(vanishingClusters, slidingGems, newGems);
        }

        internal void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            foreach (var tileOperation in operations)
            {
                Swap(tileOperation.A, tileOperation.B);
            }
        }

        Dictionary<GemColorType, List<List<Coordinate>>> DetectClusters()
        {
            var test = new Dictionary<Coordinate, bool>();
            var clusters = new Dictionary<GemColorType, List<List<Coordinate>>>();
            var specialTest = new Dictionary<Coordinate, bool>();

            foreach (var enumValue in Enum.GetValues(typeof(GemColorType)))
            {
                var gemColorType = (GemColorType) enumValue;

                test.Clear();
                clusters[gemColorType] = new List<List<Coordinate>>();

                for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
                {
                    for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                    {
                        var coordinate = new Coordinate(colIndex, rowIndex);

                        if (test.ContainsKey(coordinate))
                        {
                            continue;
                        }

                        test[coordinate] = true;

                        if (!HasTileStatusContain(coordinate, gemColorType))
                        {
                            continue;
                        }

                        var cluster = new List<Coordinate>();
                        GetClusterOfBy(cluster, coordinate, gemColorType, test, specialTest);

                        cluster.Add(coordinate);

                        if (cluster.Count < Setting.MinClusterSize)
                        {
                            continue;
                        }

                        clusters[gemColorType].Add(cluster);
                    }
                }
            }

            return clusters;
        }

        void GetClusterOfBy(List<Coordinate> outNeighbors,
                            Coordinate coordinate,
                            GemColorType gemColorType,
                            Dictionary<Coordinate, bool> test,
                            Dictionary<Coordinate, bool> specialTest)
        {
            for (var neighborIndex = 0; neighborIndex < DirectionService.NeighborSize; ++neighborIndex)
            {
                var neighbor = DirectionService.GetNeighborOf(coordinate, neighborIndex);

                if (!IsCoordinateInRange(neighbor))
                {
                    continue;
                }

                if (test.ContainsKey(neighbor))
                {
                    continue;
                }

                test[neighbor] = true;

                if (HasTileStatusContain(neighbor, GemColorType.Rainbow) && !specialTest.ContainsKey(neighbor))
                {
                    specialTest[neighbor] = true;
                    outNeighbors.Add(neighbor);
                    GetClusterOfBy(outNeighbors, neighbor, gemColorType, test, specialTest);

                    continue;
                }

                if (!HasTileStatusContain(neighbor, gemColorType))
                {
                    continue;
                }

                outNeighbors.Add(neighbor);
                GetClusterOfBy(outNeighbors, neighbor, gemColorType, test, specialTest);
            }
        }

        TileStatus GetTileStatusAt(Coordinate atCoordinate)
        {
            Assert.IsTrue(IsCoordinateInRange(atCoordinate), "IsCoordinateInRange(atCoordinate)");
            return CurrentTileStatuses[atCoordinate.Column, atCoordinate.Row];
        }

        bool HasTileStatusContain(Coordinate coordinate, GemColorType gemColorType)
        {
            return CurrentTileStatuses[coordinate.Column, coordinate.Row].GemColorType == gemColorType;
        }

        bool IsCoordinateInRange(Coordinate coordinate)
        {
            return coordinate.Column >= 0
                   && coordinate.Column < CurrentTileStatuses.GetLength(dimension: 0)
                   && coordinate.Row >= 0
                   && coordinate.Row < CurrentTileStatuses.GetLength(dimension: 1);
        }

        void MakeClustersVanish(VanishingClusters vanishingClusters)
        {
            foreach (var gemColorType in vanishingClusters.GemColorTypes)
            {
                foreach (var coordinates in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    foreach (var coordinate in coordinates)
                    {
                        CurrentTileStatuses[coordinate.Column, coordinate.Row] = TileStatus.Empty;
                    }
                }
            }
        }

        SlidingGems SlideGems(VanishingClusters vanishingClusters)
        {
            var maxRowLength = vanishingClusters.GemColorTypes
                .SelectMany(vanishingClusters.GetVanishingCoordinatesOf)
                .SelectMany(coordinates => coordinates)
                .GroupBy(coordinate => coordinate.Column)
                .Max(group => group.Count());

            var slidingGemList = new List<SlidingGems.SlidingGem>();

            var count = 0;

            while (count++ < maxRowLength)
            {
                for (var colIndex = TileSize.ColSize - 1; colIndex >= 0; --colIndex)
                {
                    for (var rowIndex = TileSize.RowSize - 1; rowIndex >= 0; --rowIndex)
                    {
                        var coordinate = new Coordinate(colIndex, rowIndex);

                        var hasGem = GetTileStatusAt(coordinate).TileStatusType == TileStatusType.Contain;
                        var below = DirectionService.GetBelow(coordinate);
                        var hasBelow = IsCoordinateInRange(below)
                                       && GetTileStatusAt(coordinate).TileStatusType == TileStatusType.Empty;

                        if (!hasGem || !hasBelow)
                        {
                            continue;
                        }

                        var slidingGem = new SlidingGems.SlidingGem(
                            GetTileStatusAt(below).GemColorType!.Value,
                            coordinate, below
                        );

                        slidingGemList.Add(slidingGem);

                        Swap(slidingGem.From, slidingGem.To);
                    }
                }
            }

            return new SlidingGems(slidingGemList);
        }

        void Swap(Coordinate a, Coordinate b)
        {
            TileStatus[,] map = CurrentTileStatuses;
            (map[a.Column, a.Row], map[b.Column, b.Row]) = (map[b.Column, b.Row], map[a.Column, a.Row]);
        }
    }
}

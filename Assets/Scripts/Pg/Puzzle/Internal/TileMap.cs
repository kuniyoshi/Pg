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
        GemGenerator GemGenerator { get; }
        Map Map { get; }

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

            Map = new Map(map);
            GemGenerator = new GemGenerator();
        }

        internal TileStatus[,] CurrentTileStatuses => Map.CurrentTileStatuses;

        internal SimulationStepData ProcessTurn()
        {
            var vanishingClusters = new VanishingClusters(DetectClusters());
            MakeClustersVanish(vanishingClusters);
            var slidingGems = SlideGems();

            return new SimulationStepData(vanishingClusters, slidingGems);
        }

        internal void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            foreach (var tileOperation in operations)
            {
                Map.Swap(tileOperation.A, tileOperation.B);
            }
        }

        Dictionary<GemColorType, List<List<Coordinate>>> DetectClusters()
        {
            var test = new Dictionary<Coordinate, bool>();
            var clusters = new Dictionary<GemColorType, List<List<Coordinate>>>();
            var specialTest = new Dictionary<Coordinate, bool>();

            foreach (var newGemColorType in GemColorType.Values)
            {
                test.Clear();
                clusters[newGemColorType] = new List<List<Coordinate>>();

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

                        if (!Map.HasTileStatusContain(coordinate, newGemColorType))
                        {
                            continue;
                        }

                        var cluster = new List<Coordinate>();
                        specialTest.Clear();
                        GetClusterOfBy(cluster, coordinate, newGemColorType, test, specialTest);

                        cluster.Add(coordinate);

                        if (cluster.Count < Setting.MinClusterSize)
                        {
                            continue;
                        }

                        clusters[newGemColorType].Add(cluster);
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

                if (!Map.IsCoordinateInRange(neighbor))
                {
                    continue;
                }

                if (test.ContainsKey(neighbor))
                {
                    continue;
                }

                test[neighbor] = true;

                if (Map.HasTileStatusContain(neighbor, GemColorType.Rainbow) && !specialTest.ContainsKey(neighbor))
                {
                    specialTest[neighbor] = true;
                    outNeighbors.Add(neighbor);
                    GetClusterOfBy(outNeighbors, neighbor, gemColorType, test, specialTest);

                    continue;
                }

                if (!Map.HasTileStatusContain(neighbor, gemColorType))
                {
                    continue;
                }

                outNeighbors.Add(neighbor);
                GetClusterOfBy(outNeighbors, neighbor, gemColorType, test, specialTest);
            }
        }

        void MakeClustersVanish(VanishingClusters vanishingClusters)
        {
            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var coordinates in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    foreach (var coordinate in coordinates)
                    {
                        Map.SetEmpty(coordinate);
                    }
                }
            }
        }

        SlidingGems SlideGems()
        {
            bool UpdateChanges(SlidingGems.Builder builder)
            {
                var didChange = false;

                for (var rowIndex = TileSize.RowSize - 1; rowIndex >= 0; --rowIndex)
                {
                    for (var colIndex = TileSize.ColSize - 1; colIndex >= 0; --colIndex)
                    {
                        var coordinate = new Coordinate(colIndex, rowIndex);

                        var isEmpty = Map.GetTileStatusAt(coordinate).TileStatusType == TileStatusType.Empty;

                        if (isEmpty && CoordinateService.IsTopRow(coordinate))
                        {
                            didChange = true;
                            var newGemColorType = GemGenerator.Next();
                            var newGem = new SlidingGems.NewGem(coordinate, newGemColorType);
                            builder.AddNewGem(newGem);
                            Map.AddGem(coordinate, newGemColorType);

                            continue;
                        }

                        if (!isEmpty)
                        {
                            continue;
                        }

                        var above = DirectionService.GetJustAbove(coordinate);

                        if (!Map.IsCoordinateInRange(above))
                        {
                            continue;
                        }

                        if (Map.GetTileStatusAt(above).TileStatusType == TileStatusType.Empty)
                        {
                            continue;
                        }

                        var candidates = new[]
                        {
                            above,
                            DirectionService.GetUpperLeft(coordinate),
                            DirectionService.GetUpperRight(coordinate),
                        };

                        var slidingGem = candidates.Select(candidate => GetSlidingGem(candidate, coordinate))
                            .FirstOrDefault(resultCandidate => resultCandidate.HasValue);

                        if (!slidingGem.HasValue)
                        {
                            continue;
                        }

                        didChange = true;

                        builder.AddSlidingGem(slidingGem.Value);
                        Map.Swap(slidingGem.Value.From, slidingGem.Value.To);

                        if (CoordinateService.IsTopRow(slidingGem.Value.From))
                        {
                            Assert.AreEqual(
                                TileStatusType.Empty, Map.GetTileStatusAt(slidingGem.Value.From).TileStatusType
                            );
                            var newGemColorType = GemGenerator.Next();
                            var newGem = new SlidingGems.NewGem(slidingGem.Value.From, newGemColorType);
                            builder.AddNewGem(newGem);
                            Map.AddGem(slidingGem.Value.From, newGemColorType);
                        }
                    }
                }

                return didChange;
            }

            SlidingGems.SlidingGem? GetSlidingGem(Coordinate candidate, Coordinate current)
            {
                var canSwap = Map.IsCoordinateInRange(candidate)
                              && Map.GetTileStatusAt(candidate).TileStatusType == TileStatusType.Contain;

                if (!canSwap)
                {
                    return null;
                }

                var slidingGem = new SlidingGems.SlidingGem(Map.GetTileStatusAt(candidate).GemColorType!,
                    candidate,
                    current
                );

                return slidingGem;
            }

            var resultBuilder = new SlidingGems.Builder();
            var safetyCounter = 0;

            while (true)
            {
                const int maxLoopCount = 1000;

                if (safetyCounter++ >= maxLoopCount)
                {
                    throw new Exception("Too may loop!");
                }

                if (!UpdateChanges(resultBuilder))
                {
                    break;
                }
            }

            return resultBuilder.Build();
        }
    }
}

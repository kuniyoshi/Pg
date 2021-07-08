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
    internal class Worker
    {
        GemGenerator GemGenerator { get; }

        internal Worker()
        {
            GemGenerator = new GemGenerator();
        }

        internal SimulationStepData ProcessTurn(Map map)
        {
            var vanishingClusters = new VanishingClusters(DetectClusters(map));
            MakeClustersVanish(map, vanishingClusters);
            var slidingGems = SlideGems(map);

            return new SimulationStepData(vanishingClusters, slidingGems);
        }

        internal void WorkTransaction(Map map, IEnumerable<TileOperation> operations)
        {
            foreach (var tileOperation in operations)
            {
                map.Swap(tileOperation.A, tileOperation.B);
            }
        }

        Dictionary<GemColorType, List<List<Coordinate>>> DetectClusters(Map map)
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

                        if (!map.HasTileStatusContain(coordinate, newGemColorType))
                        {
                            continue;
                        }

                        var cluster = new List<Coordinate>();
                        specialTest.Clear();
                        GetClusterOfBy(cluster, coordinate, newGemColorType, test, specialTest, map);

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
                            Dictionary<Coordinate, bool> specialTest,
                            Map map)
        {
            for (var neighborIndex = 0; neighborIndex < DirectionService.NeighborSize; ++neighborIndex)
            {
                var neighbor = DirectionService.GetNeighborOf(coordinate, neighborIndex);

                if (!map.IsCoordinateInRange(neighbor))
                {
                    continue;
                }

                if (test.ContainsKey(neighbor))
                {
                    continue;
                }

                test[neighbor] = true;

                if (map.HasTileStatusContain(neighbor, GemColorType.Rainbow) && !specialTest.ContainsKey(neighbor))
                {
                    specialTest[neighbor] = true;
                    outNeighbors.Add(neighbor);
                    GetClusterOfBy(outNeighbors, neighbor, gemColorType, test, specialTest, map);

                    continue;
                }

                if (!map.HasTileStatusContain(neighbor, gemColorType))
                {
                    continue;
                }

                outNeighbors.Add(neighbor);
                GetClusterOfBy(outNeighbors, neighbor, gemColorType, test, specialTest, map);
            }
        }

        void MakeClustersVanish(Map map, VanishingClusters vanishingClusters)
        {
            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var coordinates in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    foreach (var coordinate in coordinates)
                    {
                        map.SetEmpty(coordinate);
                    }
                }
            }
        }

        SlidingGems SlideGems(Map map)
        {
            bool UpdateChanges(SlidingGems.Builder builder)
            {
                var didChange = false;

                for (var rowIndex = TileSize.RowSize - 1; rowIndex >= 0; --rowIndex)
                {
                    for (var colIndex = TileSize.ColSize - 1; colIndex >= 0; --colIndex)
                    {
                        var coordinate = new Coordinate(colIndex, rowIndex);

                        var isEmpty = map.IsEmptyAt(coordinate);

                        if (isEmpty && CoordinateService.IsTopRow(coordinate))
                        {
                            didChange = true;
                            var newGemColorType = GemGenerator.Next();
                            var newGem = new SlidingGems.NewGem(coordinate, newGemColorType);
                            builder.AddNewGem(newGem);
                            map.AddGem(coordinate, newGemColorType);

                            continue;
                        }

                        if (!isEmpty)
                        {
                            continue;
                        }

                        var above = DirectionService.GetJustAbove(coordinate);

                        if (!map.IsCoordinateInRange(above))
                        {
                            continue;
                        }

                        if (map.IsEmptyAt(above))
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
                        map.Swap(slidingGem.Value.From, slidingGem.Value.To);

                        if (CoordinateService.IsTopRow(slidingGem.Value.From))
                        {
                            Assert.IsTrue(map.IsEmptyAt(slidingGem.Value.From), "map.IsEmptyAt(slidingGem.Value.From)");
                            var newGemColorType = GemGenerator.Next();
                            var newGem = new SlidingGems.NewGem(slidingGem.Value.From, newGemColorType);
                            builder.AddNewGem(newGem);
                            map.AddGem(slidingGem.Value.From, newGemColorType);
                        }
                    }
                }

                return didChange;
            }

            SlidingGems.SlidingGem? GetSlidingGem(Coordinate candidate, Coordinate current)
            {
                var canSwap = map.IsCoordinateInRange(candidate)
                              && map.GetTileStatusAt(candidate).TileStatusType == TileStatusType.Contain;

                if (!canSwap)
                {
                    return null;
                }

                var slidingGem = new SlidingGems.SlidingGem(
                    map.GetTileStatusAt(candidate).GemColorType!,
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

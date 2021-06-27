#nullable enable
using System;
using System.Collections.Generic;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;

namespace Pg.Puzzle.Internal
{
    internal class TileMap
    {
        internal TileMap(TileStatus[,] tileStatuses)
        {
            var colSize = tileStatuses.GetLength(dimension: 0);
            var rowSize = tileStatuses.GetLength(dimension: 1);
            var map = new TileStatus[colSize, rowSize];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    map[colIndex, rowIndex] = new TileStatus(
                        tileStatuses[colIndex, rowIndex].TileStatusType switch
                        {
                            TileStatusType.Closed => TileStatusType.Closed,
                            TileStatusType.Empty => TileStatusType.Empty,
                            TileStatusType.Green => TileStatusType.Contain,
                            TileStatusType.Red => TileStatusType.Contain,
                            TileStatusType.Purple => TileStatusType.Contain,
                            TileStatusType.Blue => TileStatusType.Contain,
                            TileStatusType.Yellow => TileStatusType.Contain,
                            TileStatusType.Orange => TileStatusType.Contain,
                            TileStatusType.Special => TileStatusType.Contain,
                            _ => throw new ArgumentOutOfRangeException(),
                        },
                        tileStatuses[colIndex, rowIndex].TileStatusType switch
                        {
                            TileStatusType.Closed => null,
                            TileStatusType.Empty => null,
                            TileStatusType.Green => GemColorType.Green,
                            TileStatusType.Red => GemColorType.Red,
                            TileStatusType.Purple => GemColorType.Purple,
                            TileStatusType.Blue => GemColorType.Blue,
                            TileStatusType.Yellow => GemColorType.Yellow,
                            TileStatusType.Orange => GemColorType.Orange,
                            TileStatusType.Special => GemColorType.Rainbow,
                            TileStatusType.Contain => null,
                            _ => throw new ArgumentOutOfRangeException(),
                        }
                    );
                }
            }

            CurrentTileStatuses = map;
        }

        internal TileStatus[,] CurrentTileStatuses { get; }

        internal SimulationStepData ProcessTurn()
        {
            var clusters = DetectClusters();

            return new SimulationStepData(new VanishingClusters(clusters));
        }

        internal void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            static void Swap(TileStatus[,] map, Coordinate a, Coordinate b)
            {
                (map[a.Column, a.Row], map[b.Column, b.Row]) = (map[b.Column, b.Row], map[a.Column, a.Row]);
            }

            foreach (var tileOperation in operations)
            {
                Swap(CurrentTileStatuses, tileOperation.A, tileOperation.B);
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

        bool HasTileStatusContain(Coordinate coordinate, GemColorType gemColorType)
        {
            return CurrentTileStatuses[coordinate.Column, coordinate.Row].GemColorType != gemColorType;
        }

        bool IsCoordinateInRange(Coordinate coordinate)
        {
            return coordinate.Column >= 0
                   && coordinate.Column < CurrentTileStatuses.GetLength(dimension: 0)
                   && coordinate.Row >= 0
                   && coordinate.Row < CurrentTileStatuses.GetLength(dimension: 1);
        }
    }
}

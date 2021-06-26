using System;
using System.Collections.Generic;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;

namespace Pg.Puzzle.Internal
{
    internal class TileMap
    {
        internal TileMap(TileStatusType[,] tileStatusTypes, TileStatus[,] x)
        {
            var colSize = tileStatusTypes.GetLength(dimension: 0);
            var rowSize = tileStatusTypes.GetLength(dimension: 1);
            var colSize2 = x.GetLength(dimension: 0);
            var rowSize2 = x.GetLength(dimension: 1);
            var map = new TileStatusType[colSize, rowSize];
            var map2 = new TileStatus[colSize2, rowSize2];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    map[colIndex, rowIndex] = tileStatusTypes[colIndex, rowIndex];
                    map2[colIndex, rowIndex] = new TileStatus(
                        x[colIndex, rowIndex].TileStatusType switch
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
                        x[colIndex, rowIndex].TileStatusType switch
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

            Tiles = map2;
        }

        internal TileStatus[,] Tiles { get; }

        internal SimulationStepData ProcessTurn()
        {
            var colorStatuses = TileStatusService.GetColorStatusesExceptSpecial();
            var test = new Dictionary<Coordinate, bool>();
            var clusters = new Dictionary<TileStatusType, List<List<Coordinate>>>();
            var specialTest = new Dictionary<Coordinate, bool>();

            foreach (var colorStatus in colorStatuses)
            {
                test.Clear();
                clusters[colorStatus] = new List<List<Coordinate>>();

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

                        if (IsTileStatusAt(coordinate, colorStatus))
                        {
                            continue;
                        }

                        var cluster = new List<Coordinate>();
                        GetClusterOfBy(cluster, coordinate, colorStatus, test, specialTest);

                        cluster.Add(coordinate);

                        if (cluster.Count < Setting.MinClusterSize)
                        {
                            continue;
                        }

                        clusters[colorStatus].Add(cluster);
                    }
                }
            }

            return new SimulationStepData(new VanishingClusters(clusters));
        }

        internal void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            foreach (var tileOperation in operations)
            {
                var (a, b) = (tileOperation.A, tileOperation.B);

                (Tiles[a.Column, a.Row], Tiles[b.Column, b.Row]) = (Tiles[b.Column, b.Row], Tiles[a.Column, a.Row]);
            }
        }

        void GetClusterOfBy(List<Coordinate> outNeighbors,
                            Coordinate coordinate,
                            TileStatusType colorStatusType,
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

                if (IsTileStatusAt(neighbor, TileStatusType.Special) && !specialTest.ContainsKey(neighbor))
                {
                    specialTest[neighbor] = true;
                    outNeighbors.Add(neighbor);
                    GetClusterOfBy(outNeighbors, neighbor, colorStatusType, test, specialTest);

                    continue;
                }

                if (!IsTileStatusAt(neighbor, colorStatusType))
                {
                    continue;
                }

                outNeighbors.Add(neighbor);
                GetClusterOfBy(outNeighbors, neighbor, colorStatusType, test, specialTest);
            }
        }

        bool IsCoordinateInRange(Coordinate coordinate)
        {
            return coordinate.Column >= 0
                   && coordinate.Column < Tiles.GetLength(dimension: 0)
                   && coordinate.Row >= 0
                   && coordinate.Row < Tiles.GetLength(dimension: 1);
        }

        bool IsTileStatusAt(Coordinate coordinate, TileStatusType colorStatus)
        {
            return Tiles[coordinate.Column, coordinate.Row].TileStatusType != colorStatus;
        }
    }
}

using System.Collections.Generic;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;

namespace Pg.Puzzle.Internal
{
    internal class TileMap
    {
        static bool IsCoordinateInRange(TileStatusType[,] tileStatusTypes, Coordinate coordinate)
        {
            return coordinate.Column >= 0
                   && coordinate.Column < tileStatusTypes.GetLength(dimension: 0)
                   && coordinate.Row >= 0
                   && coordinate.Row < tileStatusTypes.GetLength(dimension: 1);
        }

        static TileStatusType TilesAt(Coordinate coordinate, TileStatusType[,] tileStatusTypes)
        {
            return tileStatusTypes[coordinate.Column, coordinate.Row];
        }

        internal TileMap(TileStatusType[,] tileStatusTypes)
        {
            var map = new TileStatusType[tileStatusTypes.GetLength(dimension: 0),
                tileStatusTypes.GetLength(dimension: 1)];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    map[colIndex, rowIndex] = tileStatusTypes[colIndex, rowIndex];
                }
            }

            Tiles = map;
        }

        internal TileStatusType[,] Tiles { get; }

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

                        if (TilesAt(coordinate, Tiles) != colorStatus)
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

                if (!IsCoordinateInRange(Tiles, neighbor))
                {
                    continue;
                }

                if (test.ContainsKey(neighbor))
                {
                    continue;
                }

                test[neighbor] = true;

                if (TilesAt(neighbor, Tiles) == TileStatusType.Special && !specialTest.ContainsKey(neighbor))
                {
                    specialTest[neighbor] = true;
                    outNeighbors.Add(neighbor);
                    GetClusterOfBy(outNeighbors, neighbor, colorStatusType, test, specialTest);

                    continue;
                }

                if (TilesAt(neighbor, Tiles) != colorStatusType)
                {
                    continue;
                }

                outNeighbors.Add(neighbor);
                GetClusterOfBy(outNeighbors, neighbor, colorStatusType, test, specialTest);
            }
        }
    }
}

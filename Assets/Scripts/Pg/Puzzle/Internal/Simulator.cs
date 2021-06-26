#nullable enable
using System.Collections.Generic;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    public class Simulator
    {
        public Simulator(IGameData gameData)
        {
            var tileStatuses = gameData.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Tiles = new TileStatus[TileSize.ColSize, TileSize.RowSize];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    Tiles[colIndex, rowIndex] = tileStatuses[colIndex, rowIndex];
                }
            }
        }

        internal TileStatus[,] Tiles { get; }

        public SimulationStepData ProcessTurn()
        {
            var colorStatuses = TileStatusService.GetColorStatusesExceptSpecial();
            var test = new Dictionary<Coordinate, bool>();
            var clusters = new Dictionary<TileStatus, List<List<Coordinate>>>();
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

                        if (TilesAt(coordinate) != colorStatus)
                        {
                            continue;
                        }

                        var cluster = new List<Coordinate>();
                        GetClusterOfBy(cluster, coordinate, test, specialTest, colorStatus);

                        cluster.Add(coordinate);

                        if (cluster.Count < Setting.MinClusterSize)
                        {
                            continue;
                        }

                        clusters[colorStatus].Add(cluster);
                    }
                }
            }

            return new SimulationStepData
            {
                Clusters = new Clusters(clusters),
            };
        }

        public void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            foreach (var tileOperation in operations)
            {
                var (a, b) = (tileOperation.A, tileOperation.B);

                (Tiles[a.Column, a.Row], Tiles[b.Column, b.Row]) = (Tiles[b.Column, b.Row], Tiles[a.Column, a.Row]);
            }
        }

        void GetClusterOfBy(List<Coordinate> outNeighbors,
                            Coordinate coordinate,
                            Dictionary<Coordinate, bool> test,
                            Dictionary<Coordinate, bool> specialTest,
                            TileStatus colorStatus)
        {
            for (var i = 0; i < DirectionService.NeighborSize; ++i)
            {
                var neighbor = DirectionService.GetNeighborOf(coordinate, i);

                if (!IsCoordinateInRange(neighbor))
                {
                    continue;
                }

                if (test.ContainsKey(neighbor))
                {
                    continue;
                }

                test[neighbor] = true;

                if (TilesAt(neighbor) == TileStatus.Special && !specialTest.ContainsKey(neighbor))
                {
                    specialTest[neighbor] = true;
                    outNeighbors.Add(neighbor);
                    GetClusterOfBy(outNeighbors, neighbor, test, specialTest, colorStatus);

                    continue;
                }

                if (TilesAt(neighbor) != colorStatus)
                {
                    continue;
                }

                outNeighbors.Add(neighbor);
                GetClusterOfBy(outNeighbors, neighbor, test, specialTest, colorStatus);
            }
        }

        bool IsCoordinateInRange(Coordinate coordinate)
        {
            return coordinate.Column >= 0
                   && coordinate.Column < Tiles.GetLength(dimension: 0)
                   && coordinate.Row >= 0
                   && coordinate.Row < Tiles.GetLength(dimension: 1);
        }

        TileStatus TilesAt(Coordinate coordinate)
        {
            return Tiles[coordinate.Column, coordinate.Row];
        }
    }
}

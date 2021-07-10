#nullable enable
using System.Collections.Generic;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using Pg.Rule;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class Simulator
    {
        Map Map { get; }
        ScoreCalculator ScoreCalculator { get; }
        Worker Worker { get; }

        internal Simulator(IGameData gameData)
        {
            var tileStatuses = gameData.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Map = new Map(tileStatuses);
            Worker = new Worker();
            ScoreCalculator = new ScoreCalculator();
        }

        internal TileStatus[,] CurrentTileStatuses => Map.CurrentTileStatuses;

        internal IEnumerable<SimulationStepData> ProcessTurn(IEnumerable<TileOperation> operations)
        {
            ScoreCalculator.Clear();

            foreach (var (tileStatuses, vanishingClusters, slidingGems) in Worker.ProcessTurn(Map, operations))
            {
                yield return new SimulationStepData(
                    tileStatuses,
                    vanishingClusters,
                    slidingGems,
                    ScoreCalculator.StepCalculate(vanishingClusters)
                );
            }
        }
    }
}

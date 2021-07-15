#nullable enable
using System.Collections.Generic;
using Pg.Data.Request;
using Pg.Data.Response;
using Pg.Data.Simulation;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using Pg.Rule;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class Simulator
    {
        GameEndJudgement GameEndJudgement { get; }
        Map Map { get; }

        ScoreBoard ScoreBoard { get; }
        Turn Turn { get; }
        Worker Worker { get; }

        internal Simulator(IGameData gameData)
        {
            var stage = gameData.Stage;
            var tileStatuses = stage.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Map = new Map(tileStatuses);
            Worker = new Worker();
            Turn = new Turn();
            ScoreBoard = new ScoreBoard();
            GameEndJudgement = new GameEndJudgement(stage.MaxTurnCount, stage.TargetScore);
        }

        internal TileStatus[,] CurrentTileStatuses => Map.CurrentTileStatuses;

        internal PassedTurn PassedTurn => Turn.PassedTurn;

        internal TurnResponse ProcessTurn(IEnumerable<TileOperation> operations)
        {
            var simulationStepDataItems = Worker.ProcessTurn(Map, operations);

            var simulationStepResponses = new List<SimulationStepResponse>();
            var chainingCount = 0;

            foreach (var data in simulationStepDataItems)
            {
                var score = CalculateScore.StepCalculate(
                    data.VanishingClusters,
                    new ChainingCount(chainingCount++),
                    Turn.PassedTurn
                );

                simulationStepResponses.Add(new SimulationStepResponse(data, score));
            }

            simulationStepResponses.ForEach(response => ScoreBoard.Add(response.AcquisitionScore));

            Turn.Increment();

            return new TurnResponse(
                simulationStepResponses,
                ScoreBoard.Current,
                GameEndJudgement.Judge(PassedTurn, ScoreBoard.Current)
            );
        }
    }
}

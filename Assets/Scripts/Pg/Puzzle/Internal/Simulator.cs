#nullable enable
using System.Collections.Generic;
using Pg.Data.Response;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class Simulator
    {
        Map Map { get; }
        Turn Turn { get; }
        Worker Worker { get; }

        internal Simulator(IGameData gameData)
        {
            var tileStatuses = gameData.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Map = new Map(tileStatuses);
            Worker = new Worker();
            Turn = new Turn();
        }

        internal TileStatus[,] CurrentTileStatuses => Map.CurrentTileStatuses;

        internal PassedTurn PassedTurn => Turn.PassedTurn;

        internal IEnumerable<SimulationStepData> ProcessTurn(IEnumerable<TileOperation> operations)
        {
            var result = Worker.ProcessTurn(Map, operations);
            Turn.Increment();

            return result;
        }
    }
}

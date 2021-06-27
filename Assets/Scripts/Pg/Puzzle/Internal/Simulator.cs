#nullable enable
using System.Collections.Generic;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class Simulator
    {
        internal Simulator(IGameData gameData)
        {
            var tileStatuses = gameData.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Map = new TileMap(tileStatuses);
        }

        internal TileMap Map { get; }

        internal SimulationStepData ProcessTurn()
        {
            return Map.ProcessTurn();
        }

        internal void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            Map.WorkTransaction(operations);
        }
    }
}

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
        Map Map { get; }
        TileMap TileMap { get; }

        internal Simulator(IGameData gameData)
        {
            var tileStatuses = gameData.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Map = Map.CreateMap(tileStatuses);
            TileMap = new TileMap();
        }

        internal TileStatus[,] CurrentTileStatuses => Map.CurrentTileStatuses;

        internal SimulationStepData ProcessTurn()
        {
            return TileMap.ProcessTurn(Map);
        }

        internal void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            TileMap.WorkTransaction(Map, operations);
        }
    }
}

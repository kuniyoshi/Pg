#nullable enable
using System.Collections.Generic;
using Pg.Puzzle.Internal;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;

namespace Pg.Puzzle
{
    public class GameController
    {
        Simulator Simulator { get; }

        public GameController(IGameData gameData)
        {
            Simulator = new Simulator(gameData);
        }

        public TileStatus[,] DebugGetTileStatuses()
        {
            return Simulator.CurrentTileStatuses;
        }

        public SimulationStepData ProcessTurn()
        {
            return Simulator.ProcessTurn();
        }

        public TileStatus[,] StartGame()
        {
            return Simulator.CurrentTileStatuses;
        }

        public TileStatus[,] WorkTransaction(IEnumerable<TileOperation> operations)
        {
            Simulator.WorkTransaction(operations);

            return Simulator.CurrentTileStatuses;
        }
    }
}

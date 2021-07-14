#nullable enable
using System.Collections.Generic;
using Pg.Data.Request;
using Pg.Data.Response;
using Pg.Data.Simulation;
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

        public PassedTurn PassedTurn => Simulator.PassedTurn;

        public TileStatus[,] DebugGetTileStatuses()
        {
            return Simulator.CurrentTileStatuses;
        }

        public TurnResponse ProcessTurn(IEnumerable<TileOperation> operations)
        {
            return Simulator.ProcessTurn(operations);
        }

        public TileStatus[,] StartGame()
        {
            return Simulator.CurrentTileStatuses;
        }
    }
}

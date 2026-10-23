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

        public GameController(IGameData gameData, IReadOnlyList<GemColorType> replenishmentGems)
        {
            Simulator = new Simulator(gameData, replenishmentGems);
        }

        // Minimum buffer size; chains may consume more, in which case the buffer repeats.
        public static int CalculateRequiredGemCount(IStage stage)
        {
            return checked(stage.TileStatuses.Length * stage.MaxTurnCount);
        }

        // Capture all mutable state before starting work on another thread.
        public System.Threading.Tasks.Task<HintResult?> FindHintAsync()
        {
            var search = Simulator.CreateHintSearch();
            return System.Threading.Tasks.Task.Run(() => search.Find());
        }

        public PassedTurn PassedTurn => Simulator.PassedTurn;

        public TileStatus[,] DebugGetTileStatuses()
        {
            return Simulator.CurrentTileStatuses;
        }

        public TurnResponse ProcessTurn(IEnumerable<GemOperation> operations)
        {
            return Simulator.ProcessTurn(operations);
        }

        public TileStatus[,] StartGame()
        {
            return Simulator.CurrentTileStatuses;
        }
    }
}

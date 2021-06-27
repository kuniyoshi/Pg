#nullable enable
using System.Collections.Generic;
using Pg.Puzzle.Internal;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using UnityEngine;

namespace Pg.Puzzle
{
    public static class GameController
    {
        static Simulator? _simulator;

        public static bool CanSwap(TileData a, TileData b)
        {
            return TileStatusService.CanBothBeSwappable(a.TileStatus, b.TileStatus)
                   && DirectionService.IsNeighborEachOther(a.Coordinate, b.Coordinate);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void InitializeOnReloadDomain()
        {
            _simulator = null;
        }

        public static SimulationStepData ProcessTurn()
        {
            return _simulator!.ProcessTurn();
        }

        public static TileStatus[,] StartGame(IGameData gameData)
        {
            _simulator = new Simulator(gameData);

            return _simulator!.CurrentTileStatuses;
        }

        public static TileStatus[,] WorkTransaction(IEnumerable<TileOperation> operations)
        {
            _simulator!.WorkTransaction(operations);

            return _simulator!.CurrentTileStatuses;
        }
    }
}

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

        public static void StartGame(IGameData gameData)
        {
            _simulator = new Simulator(gameData);
        }

        public static void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            _simulator!.WorkTransaction(operations);
        }

        public static TileStatus[,] CurrentTileStatuses => _simulator!.Map.CurrentTileStatuses;
    }
}

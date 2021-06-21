#nullable enable
using Pg.Etc.Puzzle;
using Pg.Puzzle.Internal;
using UnityEngine;

namespace Pg.Puzzle
{
    public static class GameController
    {
        static Simulator? _simulator;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void InitializeOnReloadDomain()
        {
            _simulator = null;
        }

        public static void StartGame(IGameData gameData)
        {
            _simulator = new Simulator(gameData);
        }

        public static bool CanSwap(TileData a, TileData b)
        {
            return TileStatusService.CanBothBeSwappable(a.TileStatus, b.TileStatus)
                   && DirectionService.IsNeighborEachOther(a.Coordinate, b.Coordinate);
        }

        public static TileStatus[,] Tiles => _simulator!.Tiles;
    }
}

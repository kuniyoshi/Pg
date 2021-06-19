#nullable enable
using System;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Internal;
using UnityEngine;

namespace Pg.Puzzle
{
    public static class GameController
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void InitializeOnReloadDomain()
        {
            _simulator = null;
        }



        static Simulator? _simulator;

        public static void StartGame(IGameData gameData)
        {
            _simulator = new Simulator(gameData);
        }

        public static TileStatus[,] Tiles => _simulator!.Tiles;
    }
}

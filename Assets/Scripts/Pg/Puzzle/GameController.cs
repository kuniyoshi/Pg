#nullable enable
using System;
using Pg.Etc.Puzzle;
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

        public static void StartGame()
        {
            _simulator = new Simulator();
        }

        public static TileStatus[,] Tiles => _simulator!.Tiles;
    }
}

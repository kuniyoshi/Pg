#nullable enable
using System;

namespace Pg.Puzzle
{
    public static class GameController
    {

        static Simulator? _simulator;

        public static void StartGame()
        {
            _simulator = new Simulator();
        }
    }
}

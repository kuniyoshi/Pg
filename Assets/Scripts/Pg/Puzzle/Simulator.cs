#nullable enable
using Pg.Etc.Puzzle;

namespace Pg.Puzzle
{
    public class Simulator
    {
        public Simulator()
        {
            Tiles = new TileStatus[TileSize.RowSize, TileSize.ColSize];
        }

        internal TileStatus[,] Tiles { get; }
    }
}

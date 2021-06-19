#nullable enable
using Pg.Etc.Puzzle;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    public class Simulator
    {
        public Simulator(IGameData gameData)
        {
            var tileStatuses = gameData.TileStatuses;
            Assert.AreEqual(TileSize.ColSize, tileStatuses.GetLength(dimension: 0));
            Assert.AreEqual(TileSize.RowSize, tileStatuses.GetLength(dimension: 1));

            Tiles = new TileStatus[TileSize.ColSize, TileSize.RowSize];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    Tiles[colIndex, rowIndex] = tileStatuses[colIndex, rowIndex];
                }
            }
        }

        internal TileStatus[,] Tiles { get; }
    }
}

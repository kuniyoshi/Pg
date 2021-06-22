#nullable enable
using System.Collections.Generic;
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

        public void WorkTransaction(IEnumerable<TileOperation> operations)
        {
            foreach (var tileOperation in operations)
            {
                var (a, b) = (tileOperation.A, tileOperation.B);

                (Tiles[a.Column, a.Row], Tiles[b.Column, b.Row]) = (Tiles[b.Column, b.Row], Tiles[a.Column, a.Row]);
            }
        }
    }
}

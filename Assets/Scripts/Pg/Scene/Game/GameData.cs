#nullable enable
using Pg.Etc.Puzzle;
using Pg.Puzzle.Internal;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Pg/GameData", order = 0)]
    public class GameData
        : ScriptableObject, IGameData
    {
        [SerializeField]
        TileStatus[]? TileStatusesRow0;

        [SerializeField]
        TileStatus[]? TileStatusesRow1;

        [SerializeField]
        TileStatus[]? TileStatusesRow2;

        [SerializeField]
        TileStatus[]? TileStatusesRow3;

        [SerializeField]
        TileStatus[]? TileStatusesRow4;

        [SerializeField]
        TileStatus[]? TileStatusesRow5;

        public TileStatus[,] TileStatuses => CreateTileStatuses();

        TileStatus[,] CreateTileStatuses()
        {
            var result = new TileStatus[TileSize.ColSize, TileSize.RowSize];

            var rows = new[]
            {
                TileStatusesRow0!,
                TileStatusesRow1!,
                TileStatusesRow2!,
                TileStatusesRow3!,
                TileStatusesRow4!,
                TileStatusesRow5!,
            };

            Assert.AreEqual(TileSize.RowSize, rows.Length);

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < rows.Length; ++rowIndex)
                {
                    result[colIndex, rowIndex] = rows[rowIndex][colIndex];
                }
            }

            return result;
        }
    }
}

#nullable enable
using Pg.Etc.Puzzle;
using Pg.Puzzle;
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
        TileStatusType[]? TileStatusesRow0;

        [SerializeField]
        TileStatusType[]? TileStatusesRow1;

        [SerializeField]
        TileStatusType[]? TileStatusesRow2;

        [SerializeField]
        TileStatusType[]? TileStatusesRow3;

        [SerializeField]
        TileStatusType[]? TileStatusesRow4;

        [SerializeField]
        TileStatusType[]? TileStatusesRow5;

        public TileStatusType[,] TileStatuses => CreateTileStatuses();

        TileStatusType[,] CreateTileStatuses()
        {
            var result = new TileStatusType[TileSize.ColSize, TileSize.RowSize];

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

        #region debug

        [ContextMenu("DebugPlusOne")]
        void DebugPlusOne()
        {
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
                    var current = rows[rowIndex][colIndex];
                    var next = current + 1;
                    rows[rowIndex][colIndex] = next;
                }
            }
        }

        #endregion
    }
}

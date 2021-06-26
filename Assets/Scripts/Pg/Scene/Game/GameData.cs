#nullable enable
using System;
using Pg.Etc.Puzzle;
using Pg.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Pg/GameData", order = 0)]
    public class GameData
        : ScriptableObject, IGameData
    {
        [SerializeField]
        TileGemType[]? TileStatusesRow0;

        [SerializeField]
        TileGemType[]? TileStatusesRow1;

        [SerializeField]
        TileGemType[]? TileStatusesRow2;

        [SerializeField]
        TileGemType[]? TileStatusesRow3;

        [SerializeField]
        TileGemType[]? TileStatusesRow4;

        [SerializeField]
        TileGemType[]? TileStatusesRow5;

        public TileStatusType[,] TileStatusesA => CreateTileStatusesA();

        public TileStatus[,] TileStatuses => CreateTileStatuses();

        TileStatus[,] CreateTileStatuses()
        {
            var tileStatusTypes = CreateTileStatusesA();
            var result = new TileStatus[tileStatusTypes.GetLength(dimension: 0),
                tileStatusTypes.GetLength(dimension: 1)];

            for (var colIndex = 0; colIndex < tileStatusTypes.GetLength(dimension: 0); ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < tileStatusTypes.GetLength(dimension: 1); ++rowIndex)
                {
                    result[colIndex, rowIndex] = new TileStatus(
                        tileStatusTypes[colIndex, rowIndex],
                        tileStatusTypes[colIndex, rowIndex] switch
                        {
                            TileStatusType.Closed => null,
                            TileStatusType.Empty => null,
                            TileStatusType.Green => GemColorType.Green,
                            TileStatusType.Red => GemColorType.Red,
                            TileStatusType.Purple => GemColorType.Purple,
                            TileStatusType.Blue => GemColorType.Blue,
                            TileStatusType.Yellow => GemColorType.Yellow,
                            TileStatusType.Orange => GemColorType.Orange,
                            TileStatusType.Special => GemColorType.Rainbow,
                            _ => throw new ArgumentOutOfRangeException(),
                        }
                    );
                }
            }

            return result;
        }

        TileStatusType[,] CreateTileStatusesA()
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
                    result[colIndex, rowIndex] = (TileStatusType) rows[rowIndex][colIndex]; // NOTE: the cast
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

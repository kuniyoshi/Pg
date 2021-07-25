#nullable enable
using System;
using Pg.Data.Request;
using Pg.Data.Simulation;
using Pg.Etc.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game.Public
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

        [SerializeField]
        int MaxTurnCount;

        [SerializeField]
        int TargetScore;

        public IStage Stage => CreateStage();

        IStage CreateStage()
        {
            return new Stage(CreateTileStatuses(), MaxTurnCount, TargetScore);
        }

        TileStatus[,] CreateTileStatuses()
        {
            static GemColorType? ConvertToGemColorType(TileGemType tileGemType)
            {
                return tileGemType switch
                {
                    TileGemType.Closed => null,
                    TileGemType.Empty => null,
                    TileGemType.ContainsGreen => GemColorType.Green,
                    TileGemType.ContainsRed => GemColorType.Red,
                    TileGemType.ContainsPurple => GemColorType.Purple,
                    TileGemType.ContainsBlue => GemColorType.Blue,
                    TileGemType.ContainsYellow => GemColorType.Yellow,
                    TileGemType.ContainsOrange => GemColorType.Orange,
                    TileGemType.ContainsRainbow => GemColorType.Rainbow,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }

            static TileStatusType ConvertToTileStatusType(TileGemType tileGemType)
            {
                return tileGemType switch
                {
                    TileGemType.Closed => TileStatusType.Closed,
                    TileGemType.Empty => TileStatusType.Empty,
                    TileGemType.ContainsGreen => TileStatusType.Contain,
                    TileGemType.ContainsRed => TileStatusType.Contain,
                    TileGemType.ContainsPurple => TileStatusType.Contain,
                    TileGemType.ContainsBlue => TileStatusType.Contain,
                    TileGemType.ContainsYellow => TileStatusType.Contain,
                    TileGemType.ContainsOrange => TileStatusType.Contain,
                    TileGemType.ContainsRainbow => TileStatusType.Contain,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }

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
                    var tileGemType = rows[rowIndex][colIndex];

                    result[colIndex, rowIndex] = new TileStatus(ConvertToTileStatusType(tileGemType),
                        ConvertToGemColorType(tileGemType)
                    );
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

#nullable enable
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal static class TileStatusService
    {
        internal static bool CanBothBeSwappable(TileStatus a, TileStatus b)
        {
            var canBothBeSwappable = IsTileStatusSwappable(a)
                                     && IsTileStatusSwappable(b);

            return canBothBeSwappable;
        }

        internal static TileStatusType[] GetColorStatusesExceptSpecial()
        {
            return new[]
            {
                TileStatusType.Green,
                TileStatusType.Red,
                TileStatusType.Purple,
                TileStatusType.Blue,
                TileStatusType.Yellow,
                TileStatusType.Orange,
            };
        }

        static bool IsTileStatusSwappable(TileStatus tileStatus)
        {
            var can = tileStatus.TileStatusType != TileStatusType.Closed
                      && tileStatus.TileStatusType != TileStatusType.Empty;
            Assert.IsTrue(!(can ^ tileStatus.GemColorType.HasValue), "!(can ^ tileStatus.GemColorType.HasValue)");

            return can;
        }
    }
}

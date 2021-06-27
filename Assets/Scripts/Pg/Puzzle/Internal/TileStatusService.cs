#nullable enable
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal static class TileStatusService
    {
        internal static bool CanBothBeSwappable(TileStatus a, TileStatus b)
        {
            return IsTileStatusSwappable(a)
                   && IsTileStatusSwappable(b);
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

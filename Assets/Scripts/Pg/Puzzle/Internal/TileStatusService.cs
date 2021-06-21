using Pg.Etc.Puzzle;

namespace Pg.Puzzle.Internal
{
    internal static class TileStatusService
    {
        static bool IsTileStatusSwappable(TileStatus tileStatus)
        {
            return tileStatus != TileStatus.Closed && tileStatus != TileStatus.Empty;
        }

        public static bool CanBothBeSwappable(TileStatus a, TileStatus b)
        {
            var canBothBeSwappable = IsTileStatusSwappable(a)
                                     && IsTileStatusSwappable(b);

            return canBothBeSwappable;
        }
    }
}

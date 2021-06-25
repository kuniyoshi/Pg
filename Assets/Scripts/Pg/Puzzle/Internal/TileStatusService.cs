#nullable enable
namespace Pg.Puzzle.Internal
{
    internal static class TileStatusService
    {
        public static bool CanBothBeSwappable(TileStatus a, TileStatus b)
        {
            var canBothBeSwappable = IsTileStatusSwappable(a)
                                     && IsTileStatusSwappable(b);

            return canBothBeSwappable;
        }

        public static TileStatus[] GetColorStatusesExceptSpecial()
        {
            return new[]
            {
                TileStatus.Green,
                TileStatus.Red,
                TileStatus.Purple,
                TileStatus.Blue,
                TileStatus.Yellow,
                TileStatus.Orange,
            };
        }

        static bool IsTileStatusSwappable(TileStatus tileStatus)
        {
            return tileStatus != TileStatus.Closed && tileStatus != TileStatus.Empty;
        }
    }
}

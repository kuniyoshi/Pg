#nullable enable
namespace Pg.Puzzle.Internal
{
    internal static class TileStatusService
    {
        public static bool CanBothBeSwappable(TileStatusType a, TileStatusType b)
        {
            var canBothBeSwappable = IsTileStatusSwappable(a)
                                     && IsTileStatusSwappable(b);

            return canBothBeSwappable;
        }

        public static TileStatusType[] GetColorStatusesExceptSpecial()
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

        static bool IsTileStatusSwappable(TileStatusType tileStatusType)
        {
            return tileStatusType != TileStatusType.Closed && tileStatusType != TileStatusType.Empty;
        }
    }
}

#nullable enable
namespace Pg.Puzzle
{
    public readonly struct TileStatus
    {
        public static TileStatus Empty = new TileStatus(
            TileStatusType.Empty,
            gemColorType: null
        );

        public TileStatus(TileStatusType tileStatusType, GemColorType? gemColorType)
        {
            TileStatusType = tileStatusType;
            GemColorType = gemColorType;
        }

        public TileStatusType TileStatusType { get; }

        public GemColorType? GemColorType { get; }
    }
}

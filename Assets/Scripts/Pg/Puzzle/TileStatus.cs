#nullable enable
using UnityEngine.Assertions;

namespace Pg.Puzzle
{
    public readonly struct TileStatus
    {
        public static TileStatus Empty { get; } = new TileStatus(
            TileStatusType.Empty,
            gemColorType: null
        );

        public TileStatus(TileStatusType tileStatusType, GemColorType? gemColorType)
        {
            Assert.IsTrue(
                !((tileStatusType == TileStatusType.Contain) ^ gemColorType.HasValue),
                "!(tileStatusType == TileStatusType.Contain ^ gemColorType.HasValue)"
            );
            TileStatusType = tileStatusType;
            GemColorType = gemColorType;
        }

        public TileStatusType TileStatusType { get; }

        public GemColorType? GemColorType { get; }
    }
}

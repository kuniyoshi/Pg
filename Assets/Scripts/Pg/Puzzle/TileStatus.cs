#nullable enable
using UnityEngine.Assertions;

namespace Pg.Puzzle
{
    public readonly struct TileStatus
    {
        public static TileStatus Empty { get; } = new TileStatus(TileStatusType.Empty, gemColorType: null);

        public TileStatus(TileStatusType tileStatusType, GemColorType? gemColorType)
        {
            var isNotNull = gemColorType != null;
            Assert.IsTrue(
                !((tileStatusType == TileStatusType.Contain) ^ (gemColorType != null)),
                "!((tileStatusType == TileStatusType.Contain) ^ (gemColorType != null))"
            );
            TileStatusType = tileStatusType;
            GemColorType = gemColorType;
        }

        public TileStatusType TileStatusType { get; }
        public GemColorType? GemColorType { get; }
        public string Sigil => GetSigil();

        string GetSigil()
        {
            return GemColorType?.Sigil ?? TileStatusType.Sigil!;
        }
    }
}

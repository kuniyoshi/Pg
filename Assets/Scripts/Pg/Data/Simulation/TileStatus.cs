#nullable enable
using UnityEngine.Assertions;

namespace Pg.Data.Simulation
{
    public readonly struct TileStatus
    {
        public static TileStatus Empty { get; } = new TileStatus(TileStatusType.Empty, gemColorType: null);

        public GemColorType? GemColorType { get; }

        public TileStatusType TileStatusType { get; }

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

        public string Sigil => GetSigil();

        string GetSigil()
        {
            return GemColorType?.Sigil ?? TileStatusType.Sigil!;
        }
    }
}

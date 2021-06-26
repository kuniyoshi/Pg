#nullable enable
namespace Pg.Puzzle
{
    public readonly struct TileData
    {
        public TileStatusType TileStatusType { get; }
        public Coordinate Coordinate { get; }

        public TileData(Coordinate coordinate, TileStatusType tileStatusType)
        {
            Coordinate = coordinate;
            TileStatusType = tileStatusType;
        }
    }
}

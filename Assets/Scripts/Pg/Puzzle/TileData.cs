#nullable enable
using Pg.Data.Simulation;

namespace Pg.Puzzle
{
    public readonly struct TileData
    {
        public TileStatus TileStatus { get; }
        public Coordinate Coordinate { get; }

        public TileData(Coordinate coordinate, TileStatus tileStatus)
        {
            Coordinate = coordinate;
            TileStatus = tileStatus;
        }
    }
}

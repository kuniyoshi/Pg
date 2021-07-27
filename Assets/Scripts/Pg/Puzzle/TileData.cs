#nullable enable
using Pg.Data.Simulation;

namespace Pg.Puzzle
{
    public readonly struct TileData
    {
        public Coordinate Coordinate { get; }
        public TileStatus TileStatus { get; }

        public TileData(Coordinate coordinate, TileStatus tileStatus)
        {
            Coordinate = coordinate;
            TileStatus = tileStatus;
        }
    }
}

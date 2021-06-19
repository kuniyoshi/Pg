#nullable enable
using Pg.Etc.Puzzle;

namespace Pg.Puzzle.Internal
{
    public interface IGameData
    {
        TileStatus[,] TileStatuses { get; }
    }
}

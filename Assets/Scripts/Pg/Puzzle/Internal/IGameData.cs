#nullable enable
namespace Pg.Puzzle.Internal
{
    public interface IGameData
    {
        TileStatus[,] TileStatuses { get; }
    }
}

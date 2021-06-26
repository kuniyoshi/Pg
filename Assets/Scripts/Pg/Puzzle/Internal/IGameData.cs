#nullable enable
namespace Pg.Puzzle.Internal
{
    public interface IGameData
    {
        TileStatusType[,] TileStatuses { get; }
    }
}

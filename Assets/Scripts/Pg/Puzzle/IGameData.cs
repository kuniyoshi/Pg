#nullable enable
namespace Pg.Puzzle
{
    public interface IGameData
    {
        TileStatusType[,] TileStatusesA { get; }
        TileStatus[,] TileStatuses { get; }
    }
}

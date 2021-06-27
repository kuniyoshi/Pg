#nullable enable
namespace Pg.Puzzle
{
    public interface IGameData
    {
        TileStatus[,] TileStatuses { get; }
    }
}

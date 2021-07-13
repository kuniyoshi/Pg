#nullable enable
namespace Pg.Data.Request
{
    public interface IGameData
    {
        TileStatus[,] TileStatuses { get; }
    }
}

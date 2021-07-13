#nullable enable
using Pg.Data.Simulation;

namespace Pg.Data.Request
{
    public interface IGameData
    {
        TileStatus[,] TileStatuses { get; }
    }
}

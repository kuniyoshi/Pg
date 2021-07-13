#nullable enable
using Pg.Data.Simulation;

namespace Pg.Data.Request
{
    public interface IStage
    {
        TileStatus[,] TileStatuses { get; }
    }
}

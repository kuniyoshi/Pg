#nullable enable
using Pg.Data.Simulation;

namespace Pg.Data.Request
{
    public interface IStage
    {
        int MaxTurnCount { get; }
        int TargetScore { get; }
        TileStatus[,] TileStatuses { get; }
    }
}

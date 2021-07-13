#nullable enable
using Pg.Data.Request;
using Pg.Data.Simulation;

namespace Pg.Scene.Game
{
    public class Stage
        : IStage
    {
        public Stage(TileStatus[,] tileStatuses)
        {
            TileStatuses = tileStatuses;
        }

        public TileStatus[,] TileStatuses { get; }
    }
}

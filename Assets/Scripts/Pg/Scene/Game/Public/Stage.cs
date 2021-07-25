#nullable enable
using Pg.Data.Request;
using Pg.Data.Simulation;

namespace Pg.Scene.Game.Public
{
    public class Stage
        : IStage
    {
        public Stage(TileStatus[,] tileStatuses,
                     int maxTurnCount,
                     int targetScore)
        {
            TileStatuses = tileStatuses;
            MaxTurnCount = maxTurnCount;
            TargetScore = targetScore;
        }

        public TileStatus[,] TileStatuses { get; }
        public int MaxTurnCount { get; }
        public int TargetScore { get; }
    }
}

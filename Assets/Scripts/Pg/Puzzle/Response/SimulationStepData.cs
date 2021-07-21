#nullable enable
using Pg.Data.Response;
using Pg.Data.Simulation;

namespace Pg.Puzzle.Response
{
    public class SimulationStepData
    {
        public TileStatus[,] BeginningMap { get; }
        public SlidingGems SlidingGems { get; }

        public VanishingClusters VanishingClusters { get; }

        public SimulationStepData(TileStatus[,] beginningMap,
                                  VanishingClusters vanishingClusters,
                                  SlidingGems slidingGems)
        {
            BeginningMap = beginningMap;
            VanishingClusters = vanishingClusters;
            SlidingGems = slidingGems;
        }
    }
}

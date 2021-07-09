#nullable enable
namespace Pg.Puzzle.Response
{
    public class SimulationStepData
    {
        public TileStatus[,] BeginningMap { get; }
        public Score Score { get; }
        public SlidingGems SlidingGems { get; }

        public VanishingClusters VanishingClusters { get; }

        public SimulationStepData(TileStatus[,] beginningMap,
                                  VanishingClusters vanishingClusters,
                                  SlidingGems slidingGems,
                                  Score score)
        {
            BeginningMap = beginningMap;
            SlidingGems = slidingGems;
            VanishingClusters = vanishingClusters;
            Score = score;
        }

        public override string ToString()
        {
            return $"{nameof(SimulationStepData)}{{"
                   + $"{nameof(VanishingClusters)}: {VanishingClusters}"
                   + $", {nameof(SlidingGems)}: [{SlidingGems.DebugDescribeHistory()}]"
                   + $", {nameof(Score)}: [{Score}]"
                   + "}";
        }
    }
}

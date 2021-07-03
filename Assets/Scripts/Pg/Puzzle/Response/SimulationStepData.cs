#nullable enable
namespace Pg.Puzzle.Response
{
    public class SimulationStepData
    {
        public SlidingGems SlidingGems { get; }

        public VanishingClusters VanishingClusters { get; }

        public SimulationStepData(VanishingClusters vanishingClusters,
                                  SlidingGems slidingGems)
        {
            SlidingGems = slidingGems;
            VanishingClusters = vanishingClusters;
        }

        public override string ToString()
        {
            return $"{nameof(SimulationStepData)}{{"
                   + $"{nameof(VanishingClusters)}: {VanishingClusters}"
                   + $", {nameof(SlidingGems)}: {SlidingGems}"
                   + "}";
        }
    }
}
